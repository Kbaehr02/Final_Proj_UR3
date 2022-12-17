using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
*Author: Kurt Baehr
*Project: Unified Robotics 2
*Purpose: Video processing, shape detection, data computation, and serial communication
*/

namespace RobotCodeUR2_V2
{
    public partial class Form1 : Form
    {
        //Declares a VideoCapture class, two seperate threads, and a serial port class
        VideoCapture capture;
        Thread captureThread;
        Thread arduinoCommsThread;
        SerialPort arduinoSerial;
        //Begins our program with the serial comms lock enabled for data safety
        bool enableCoordinateSending = false;
        //Gives initial values for blur matrix. Must always be equal and odd values.
        Size blurRatio = new Size(1, 1);
        /*
         * Global declaration of varaibles that must be accessed between scopes.
         * This is kept at a minimum to minimise memory leak opportunities.
        */
        int value;
        int triangleMinArea = 0;
        int triangleMaxArea = 0;
        int squareMinArea = 0;
        int squareMaxArea = 0;
        int borderMinArea = 0;
        int borderMaxArea = 0;
        int blur = 3;
        int pixPerIn = 0;
        /*
         * Creates a class "Shape" to store information of each shape
         * safely with all scopes in the project namespace.
         * 
         *"numb" represents where in the order of detect the shape falls.
         *"shapeType" provides a numeric value associated to its shape (triangle, square, or border).
         *"area" and "center" represent the area in pixels^2 and the (x,y) coordinates of the shapes center.
         *"drivingDist" and "drivingAngle" represent the distance and angle from a simulated robot base point to the shape.
        */
        public class Shape
        {
            public int numb { get; set; }
            public int shapeType { get; set; }
            public double area { get; set; }
            public Point center { get; set; }
            public int drivingDist { get; set; }
            public int drivingAngle { get; set; }
        }
        //creates a list called "shapeInfo" with the Shape class as its type. 
        List<Shape> shapeInfo = new List<Shape>();

        public Form1()
        {
            InitializeComponent();
        }
        //instatiates new capture for camera slot 1 and begins an image processing thread
        private void Form1_Load(object sender, EventArgs e)
        {
            capture = new VideoCapture(1);
            captureThread = new Thread(processImage);
            captureThread.Start();
        }
        /*
         * When clicked, instantiates a new serial port object and trys to open it on the user-defined port
         * The try/catch statement attempts to connect to the user-defined port. If successful, COM port status
         * Label updates. If it fails, a message box will appear notifying the user.
         * With successful connection, the arduino thread will begin.
        */
        private void openCommsButton_Click(object sender, EventArgs e)
        {
            arduinoSerial = new SerialPort();
            int comPort = Convert.ToInt32(comPortTextBox.Text);
            try
            {
                arduinoSerial.PortName = $"COM{comPort}";
                arduinoSerial.BaudRate = 9600;
                arduinoSerial.Open();
                Invoke(new Action(() =>
                {serialCommState.Text = $"Arduino is live on COM {comPort}.";}));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Initializing COM port");
            }
            arduinoCommsThread = new Thread(serialCommunication);
            arduinoCommsThread.Start();
        }
        /*
         * With the serial communication now established, the arduino's attempts to send the unlock command
         * for serial communication will go through.
         * 
         * When clicking the start button, a fresh image processing
         * thread is created. Becuase the arduino thread has made the connection, the 
         * enableCoordinateSending bool will be set true and shape accquisition will 
         * begin its loop until completion
        */
        private void startButton_Click(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                arduinoDataLabel.Text = $"\nWaiting for data...\n";
                shapeDataLabel.Text = $"Waiting for shape data...\n";
            }));
            captureThread.Abort();
            capture.Stop();
            capture = new VideoCapture(0);
            captureThread = new Thread(processImage);
            captureThread.Start();
        }
        /*
         * Below is the image processing function called by the captureThread. This function handles
         * shape detection, data computation, and updating the GUI. It calls a multitude of functions to 
         * achieve these tasks, each declared in the order of use following the serialCommunication function.
        */
        private void processImage()
        {
            while (capture.IsOpened)
            {
                //Makes sure each repetition of this function begins with fresh class storage
                shapeInfo.Clear(); 
                /*
                 * Lines 141-151 are responsible for creating and applying the needed filters to single out targets
                */
                Mat sourceMat = new Mat();
                sourceMat = capture.QueryFrame();
                capture.Pause();
                Mat countourMat = sourceMat.Clone();
                var blurredMat = new Mat();
                var binaryMat = new Mat();
                var decoratedMat = new Mat();
                CvInvoke.GaussianBlur(countourMat, blurredMat, new Size(blur, blur), 0);
                CvInvoke.CvtColor(blurredMat, blurredMat, typeof(Bgr), typeof(Gray));
                CvInvoke.Threshold(blurredMat, binaryMat, value, 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                CvInvoke.CvtColor(binaryMat, decoratedMat, typeof(Gray), typeof(Bgr));
                /*
                 * Calls functions dedicated to maintaining aspect ratio in pic boxes, target 
                 * detection, and data computation.
                */
                resizePicBoxes(sourceMat, blurredMat, binaryMat, decoratedMat);
                processShapes(binaryMat, decoratedMat);
                drivingDistsAndAngles(pixPerIn, decoratedMat);
                displayShapeData();
                /*
                 * This if statement prevents errors of trying to send data to an empty port during 
                 * initialization stage.
                */
                if (enableCoordinateSending)
                {
                    sendCoordinates();
                }
            }
        }
        /*
         * The following function is called by arduinoCommsThread. It runs in parallel with the image
         * processing and is constantly looking for the enable command from the Arduino. This function 
         * also updates the GUI via a label with the confirmation data returned from the Arduino.
        */
        private void serialCommunication()
        {
            while (true)
            {
                // block until \n character is received, extract command data
                string msg = arduinoSerial.ReadLine();
                // confirm the string has both < and > characters
                if (msg.IndexOf("<") == -1 || msg.IndexOf(">") == -1)
                {
                    continue;
                }
                // remove everything before (and including) the < character
                msg = msg.Substring(msg.IndexOf("<") + 1);
                // remove everything after (and including) the > character
                msg = msg.Remove(msg.IndexOf(">"));
                // if the resulting string is empty, disregard and move on
                if (msg.Length == 0)
                {
                    continue;
                }
                // parse the command
                if (msg.Substring(0, 1) == "S")
                {
                    // command is to suspend, toggle states accordingly:
                    ToggleFieldAvailability(msg.Substring(1, 1) == "1");
                }
                else if (msg.Substring(0, 1) == "P")
                {
                    // command is to display the point data, output to the text field:
                    Invoke(new Action(() =>
                    {
                        arduinoDataLabel.Text = $"Arduino data:\nReturned Point Data: {msg.Substring(1)}\n";
                    }));
                }
            }
        }
        /*
         * This function updates each picture box. It can handle picture boxes with varying dimensions and still 
         * maintain aspect ratio if the user decideds to modify the picture box sizes.
        */
        private void resizePicBoxes(Mat sourceMat, Mat blurredMat, Mat binaryMat, Mat decoratedMat)
        {
            int newHeight = 0;
            Size newSize = new Size(0, 0);
            Mat temp = new Mat();
            PictureBox str = new PictureBox();
            /*
             * The switch case allows template variables to be assigned the specific specifications of each 
             * picture box as the for loop steps through the cases.
            */
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            temp = sourceMat;
                            str = rawFrame;
                            break;
                        }
                    case 1:
                        {
                            temp = decoratedMat;
                            str = decoFrame;
                            break;
                        }
                    case 2:
                        {
                            temp = blurredMat;
                            str = blurFrame;
                            break;
                        }
                    case 3:
                        {
                            temp = binaryMat;
                            str = bgrFrame;
                            break;
                        }
                }
                //Dynamically updates each picture box with its respective switch condition.
                newHeight = (temp.Size.Height * str.Size.Width) / temp.Size.Width;
                newSize = new Size(str.Size.Width, newHeight);
                CvInvoke.Resize(temp, temp, newSize);
                str.Image = temp.ToBitmap();
            }
        }
        /*
         * Serving as the target identification function, each shape profile is sought after with parameters
         * determined by the user with the sliders at runtime.
        */
        private void processShapes(Mat binaryMat, Mat decoratedMat)
        {
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                //Finds every contour in the capture frame with no parameters
                CvInvoke.FindContours(binaryMat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
                //Updates in for loop each time a shape is detected and stored
                int shapes = 0;
                Invoke(new Action(() =>
                { contoursLabel.Text = $"There are {contours.Size} contours"; }));
                /*
                 * Loops through each contour to compare with parameters and determine if the contour is a valid
                 * candidate for identification.
                */
                for (int i = 0; i < contours.Size; i++)
                {
                    VectorOfPoint contour = contours[i];
                    double area = CvInvoke.ContourArea(contour);
                    if (area > triangleMinArea && area < triangleMaxArea)
                    {
                        shapes++;
                        //Creates visual outline of shape to show identification in GUI
                        CvInvoke.Polylines(decoratedMat, contour, true, new Bgr(Color.Blue).MCvScalar);
                        Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);
                        //Creates new entry to shapeInfo list.
                        Shape shape = new Shape()
                        {
                            numb = shapes,
                            shapeType = 0,
                            center = CenterCoords(boundingBox),
                            area = area
                        };
                        shapeInfo.Add(shape);
                        //Adds relevent information next to each shape in the decorated picture box.
                        MarkDetectedObject(decoFrame, decoratedMat, contours[i], boundingBox, area, shape.numb);
                    }
                    //Identical process as triangles with different area parameters
                    if (area > squareMinArea && area < squareMaxArea)
                    {
                        shapes++;
                        CvInvoke.Polylines(decoratedMat, contour, true, new Bgr(Color.Yellow).MCvScalar);
                        Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);
                        Shape shape = new Shape()
                        {
                            numb = shapes,
                            shapeType = 1,
                            center = CenterCoords(boundingBox),
                            area = area
                        };
                        shapeInfo.Add(shape);
                        MarkDetectedObject(decoFrame, decoratedMat, contours[i], boundingBox, area, shape.numb);
                    }
                    //Identical process as both shapes prior with the addition of the border information
                    if (area > borderMinArea && area < borderMaxArea)
                    {
                        shapes++;
                        CvInvoke.Polylines(decoratedMat, contour, true, new Bgr(Color.YellowGreen).MCvScalar);
                        Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);
                        pixPerIn = Convert.ToInt32(boundingBox.Width / 11);
                        Shape shape = new Shape()
                        {
                            numb = shapes,
                            shapeType = 2,
                            center = CenterCoords(boundingBox),
                            area = area
                        };
                        shapeInfo.Add(shape);
                        var info = new string[]
                        {
                            $"Border",
                            $"Center at {shape.center}",
                            $"Border height: {boundingBox.Height}",
                            $"Border width: {boundingBox.Width}",
                        };
                        WriteMultilineText(decoratedMat, info, new Point(boundingBox.Left, boundingBox.Bottom + 15));
                        MarkDetectedObject(decoFrame, decoratedMat, contours[i], boundingBox, area, shape.numb);
                    }
                }
            }
        }
        /*
         * Serving as the main computation function, the distance and angle between a virtual robot base 
         * point and the center of each shape stored in shapeInfo is computed and stored.
        */
        private void drivingDistsAndAngles(int pixPerIn, Mat frame)
        {
            /*
             * To avoid computing with non-existent data, the border must be already
             * determined or the function will exit.
            */
            Point centerBorder = new Point(0,0);
            if (shapeInfo.Count > 1)
            {
                foreach(Shape shape in shapeInfo)
                {
                    if (shape.shapeType == 2) 
                    {
                        centerBorder = shape.center;
                    }
                }
                //Creates virtual point 9 inches from the center of the border, which is the paper.
                Point robotbase = new Point(centerBorder.X, centerBorder.Y + (pixPerIn * 9));
                string str = $"Base Center @ x:{robotbase.X} y:{robotbase.Y}";
                CvInvoke.PutText(frame, str, robotbase, FontFace.HersheyPlain, 0.8, new Bgr(Color.Red).MCvScalar);
                //computes values and stores them in inches and degrees.
                for (int i = 0; i < (shapeInfo.Count); i++)
                {
                    double deltaX = shapeInfo[i].center.X - robotbase.X;
                    double deltaY = shapeInfo[i].center.Y - robotbase.Y;
                    double angleRad = Math.Atan(deltaY / deltaX);
                    double angleDeg = angleRad * 180 / Math.PI;
                    if (angleDeg < 0)
                    {
                        angleDeg = 180 + angleDeg;
                    }
                    if (angleDeg > 0)
                    {
                        shapeInfo[i].drivingAngle = Convert.ToInt32(angleDeg);
                        shapeInfo[i].drivingDist = Convert.ToInt32(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)) / pixPerIn);
                    }
                }
            }
        }
        /*
         * This function handles updating the GUI's shape data label in real time with the data
         * stored in the shapeInfo list.
        */
        private void displayShapeData()
        {
            if(shapeInfo.Count > 1)
            {
                Invoke(new Action(() =>
                {
                    string str = "Shape";
                    shapeDataLabel.Text = $"\n\nThere are {pixPerIn} pixels per virtual inch\n";
                    foreach (Shape shape in shapeInfo.ToList())
                    {
                        switch (shape.shapeType)
                        {
                            case 0:
                                str = "a Triangle";
                                break;
                            case 1:
                                str = "a Square";
                                break;
                            case 2:
                                str = "the Border";
                                break;
                        }
                        shapeDataLabel.Text += $"\nShape {shape.numb} is {str}, has an area {shape.area} and a center {shape.center}, a driving distance of {shape.drivingDist}, " +
                        $"and must rotate {shape.drivingAngle} degrees.\n";
                    }
                }));
            }
        }
        /*
         * This function handles sending data through the input buffer. It sends the driving distance and 
         * driving angle angle as seperate bytes. Each byte sent must not exceed a value larger than 255.
        */
        private void sendCoordinates()
        {
            byte[] buffer = new byte[5]
                        {
                                Encoding.ASCII.GetBytes("<")[0],
                                Convert.ToByte(shapeInfo[0].drivingAngle),
                                Convert.ToByte(shapeInfo[0].drivingDist),
                                Convert.ToByte(shapeInfo[0].shapeType),
                                Encoding.ASCII.GetBytes(">")[0]
                        };
            arduinoSerial.Write(buffer, 0, 5);
            enableCoordinateSending = false;
        }
        // This function labels each shape with its respective ID number in the shapeInfo list.
        private static void MarkDetectedObject(PictureBox decoFrame, Mat frame, VectorOfPoint contour, Rectangle boundingBox, double area, int num)
        {
            CvInvoke.Rectangle(frame, boundingBox, new Bgr(Color.DarkOrange).MCvScalar);
            Point center = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            CvInvoke.Circle(frame, center, 0, new Bgr(Color.Purple).MCvScalar, 4);
            string str = $"Shape {num}";
            CvInvoke.PutText(frame, str, new Point(center.X - 25, center.Y), FontFace.HersheyPlain, 0.8, new Bgr(Color.Red).MCvScalar);
            decoFrame.Image = frame.ToBitmap();
        }
        //A small function to avoid repeating the lengthy center formula each time it's needed.
        private Point CenterCoords(Rectangle boundingBox)
        {
            Point center = new Point(boundingBox.X + boundingBox.Width / 2, boundingBox.Y + boundingBox.Height / 2);
            return center;
        }
        /*
         * Ease of life function to avoid excessive use of new lines in strings. Will print several strings
         * stored in an array in succession at a given point.
        */
        private static void WriteMultilineText(Mat frame, string[] lines, Point origin)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                int y = i * 10 + origin.Y; // Moving down on each line
                CvInvoke.PutText(frame, lines[i], new Point(origin.X, y),
                FontFace.HersheyPlain, 0.8, new Bgr(Color.Red).MCvScalar);
            }
        }
        //Updates the serial lock label and temporarily sets enableCoordinateSending to false, locking comms.
        private void ToggleFieldAvailability(bool suspend)
        {
            Invoke(new Action(() =>
            {
                enableCoordinateSending = !suspend;
                lockStateLabel.Text = $"State: {(suspend ? "Locked" : "Unlocked")}";
            }));
        }
        //Increases blur effect
        private void gaussianUp_Click(object sender, EventArgs e)
        {
            blur = blur + 2;
            Invoke(new Action(() =>
            {
                blurLabel.Text = $"Blur matrix is {blur} by {blur}.";
            }));
        }
        //Decreases blur effect, prevents negative and even values
        private void gaussianDown_Click(object sender, EventArgs e)
        {
            if (blur >= 3)
            {
                blur = blur - 2;
                Invoke(new Action(() =>
                {
                    blurLabel.Text = $"Blur matrix is {blur} x {blur}.";
                }));
            }
            else
            {
                Invoke(new Action(() =>
                {
                    blurLabel.Text = $"Blur matrix cannot be less than 1 x 1.";
                }));
            }
        }
        //Allows for BGR filter adjustment at runtime.
        private void bgrSlider_Scroll(object sender, EventArgs e)
        {
            value = bgrSlider.Value;
            Invoke(new Action(() =>
            {
                bgrLabel.Text = $"Bgr value is {value}.";
            }));
        }
        //Dynamic modification of triangle area parameter at run time with slider
        private void triAreaSlider_Scroll(object sender, EventArgs e)
        {
            triangleMinArea = triAreaSlider.Value - 500;
            triangleMaxArea = triAreaSlider.Value + 500;
            Invoke(new Action(() =>
            {triAreaLabel.Text = $"The area parameter range for triangles is {triangleMinArea} " +
                $"to {triangleMaxArea}.";}));
        }
        //Dynamic modification of square area parameter at run time with slider
        private void squareAreaSlider_Scroll(object sender, EventArgs e)
        {
            squareMinArea = squareAreaSlider.Value - 1000;
            squareMaxArea = squareAreaSlider.Value + 1000;
            Invoke(new Action(() =>
            {squareAreaLabel.Text = $"The area parameter range for squares is {squareMinArea} " +
                $"to {squareMaxArea}.";}));
        }
        //Dynamic modification of the border area parameter at run time with slider
        private void borderAreaSlider_Scroll(object sender, EventArgs e)
        {
            borderMinArea = borderAreaSlider.Value - 5000;
            borderMaxArea = borderAreaSlider.Value + 5000;
            Invoke(new Action(() =>
            {borderAreaLabel.Text = $"The area parameter range for squares is {borderMinArea} to {borderMaxArea}.";}));
        }
        /*The arduinoDataLabel will update after a shape is sorted. When the text is changed with the returned
         * data from Arduino, this means the Arduino is finsihed and ready for a new shape, and this function
         * unlocks serial comms to listen for the enable command to which the process repeats.
        */
        private void arduinoDataLabel_TextChanged(object sender, EventArgs e)
        {
            enableCoordinateSending = true;
        }
    }
}
