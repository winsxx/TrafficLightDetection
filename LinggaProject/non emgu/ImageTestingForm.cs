using LinggaProject.support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TugasAkhir.support;
using weka.classifiers;

namespace LinggaProject
{
    public partial class ImageTestingForm : Form
    {
        private Stopwatch testImageStopwatch;
        float zoom_factor = 1;
        string test_image_filename;

        public ImageTestingForm()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Console.WriteLine("load form 1");
        }

        /* Extract features from file given by filename
        */
        private void featureExtraction(string filename)
        {
            TrafficLightFeatureExtractor extractor = new TrafficLightFeatureExtractor();
            Bitmap bmp = new Bitmap(filename);
            List<TrafficLightInstance> tl_instances = extractor.generateFromBitmap(ref bmp, true, -1);

            if (tl_instances.Count > 0)
            {

                // There are two image types
                // 1. Marked and processed (main_image)
                // 2. Original image
                Image main_image = extractor.getProcessedImage();
                testImageBox.Image = main_image;
                Image original_image = extractor.getPreservedImage();
                originalImageBox.Image = original_image;

                // Load WEKA generated model
                weka.classifiers.Classifier cl = loadModel("model");

                // Initialize table headers
                detectedListContainer.Controls.Clear();
                Label number_label = new Label();
                number_label.Text = "Number";
                detectedListContainer.Controls.Add(number_label);
                Label image_label = new Label();
                image_label.Text = "Cropped Image";
                detectedListContainer.Controls.Add(image_label);
                Label position_label = new Label();
                position_label.Text = "Position";
                detectedListContainer.Controls.Add(position_label);
                Label class_label = new Label();
                class_label.Text = "Class";
                detectedListContainer.Controls.Add(class_label);
                Label manual_class= new Label();
                manual_class.Text = "Correct Class";
                detectedListContainer.Controls.Add(manual_class);

                // Process instance by instance in the tl_instances to get Class
                int nb_instance = 0;
                int size = Constants.size;
                foreach (TrafficLightInstance tl_instance in tl_instances)
                {
                    // Precondition
                    if (tl_instance == null) continue;

                    // Make array of vals
                    double[] vals = new double[size*size*3 + 1];
                    for (int j = 0; j < size*size; j++)
                    {
                        vals[0 + j * 3] = tl_instance.colors[j].H;
                        vals[1 + j * 3] = tl_instance.colors[j].S;
                        vals[2 + j * 3] = tl_instance.colors[j].V;
                    }
                    //vals[27] = tl_instance.width;
                    //vals[28] = tl_instance.height;
                    vals[size*size*3] = tl_instance.tl_class;

                    // Make instance based on vals
                    weka.core.Instances dataUnlabeled = new weka.core.Instances("TestInstances", DatasetGenerator.initArff(), 0);
                    weka.core.Instance currentInst = new weka.core.Instance(1.0, vals);
                    dataUnlabeled.add(currentInst);
                    dataUnlabeled.setClassIndex(dataUnlabeled.numAttributes() - 1);

                    // Classify current instance
                    double predictedClass = cl.classifyInstance(dataUnlabeled.firstInstance());
                    nb_instance++;

                    // Put instance on table
                    // Number
                    Label number = new Label();
                    number.Text = "" + nb_instance;
                    // Image (cropped)
                    PictureBoxWithInterpolationMode imageBox = new PictureBoxWithInterpolationMode();
                    imageBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    imageBox.SizeMode = PictureBoxSizeMode.Zoom;
                    Bitmap image = ImageUtil.CropImage(original_image, tl_instance.x, tl_instance.y, (int)tl_instance.width, (int)tl_instance.height);
                    if (image == null)
                    {
                        nb_instance--;
                        continue;
                    }
                    imageBox.Image = image;
                    // Position
                    Label coordinate = new Label();
                    coordinate.Width = 150;
                    coordinate.Height = 40;
                    coordinate.Text = "(" + tl_instance.x + ", " + tl_instance.y + ")\n(" 
                        + (tl_instance.x + tl_instance.width) + ", " + (tl_instance.y + tl_instance.height) + ")" ;
                    // Class
                    Label classified_as = new Label();
                    classified_as.Text = "" + predictedClass;
                    // Manual Class Input
                    TextBox manual_class_input = new TextBox();
                    detectedListContainer.Controls.Add(number);
                    detectedListContainer.Controls.Add(imageBox);
                    detectedListContainer.Controls.Add(coordinate);
                    detectedListContainer.Controls.Add(classified_as);
                    detectedListContainer.Controls.Add(manual_class_input);
                }

                // Resize height to match box
                zoom_factor = (float)testImageBox.Width / (float)main_image.Width;
                float height = (float)zoom_factor * (float)main_image.Height;
                testImageBox.Height = (int)height;
                originalImageBox.Height = (int)height;
                testImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
                originalImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        // Save classifier model to path based on classifier
        public static void saveModel(String path, weka.classifiers.Classifier cl)
        {
            weka.core.SerializationHelper.write(path, cl);
        }

        // Load classifier model from path
        public static weka.classifiers.Classifier loadModel(String path)
        {
            return (weka.classifiers.Classifier)weka.core.SerializationHelper.read(path);
        }

        // Test Image Button Click
        private void testImageButton_Click(object sender, EventArgs e)
        {
            DialogResult result = imageFileDialog.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                testImageStopwatch = new Stopwatch();
                testImageStopwatch.Start();
                test_image_filename = imageFileDialog.FileName;
                button1.Text = test_image_filename.Substring(test_image_filename.LastIndexOf("\\") + 1);
                featureExtraction(test_image_filename);
                testImageStopwatch.Stop();
                explanationBox.Text = "Test image time: " + testImageStopwatch.Elapsed;
                Console.WriteLine(result); // <-- For debugging use.
            }
        }

        private void originalImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            originalImageCursor.Text = String.Format("X: {0}; Y: {1}", (int)(e.X / zoom_factor), (int)(e.Y / zoom_factor));
        }

        private void testImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            processedImageCursor.Text = String.Format("X: {0}; Y: {1}", (int)(e.X / zoom_factor), (int)(e.Y / zoom_factor));
        }
        
        public void clearExplanationText ()
        {
            explanationBox.Text = "";
        }

        public void addExplanationText (string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(addExplanationText), new object[] { text });
                return;
            }
            explanationBox.AppendText(text + System.Environment.NewLine);
        }

        public void setElementStatus (Control element, bool isEnabled)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Control, bool>(setElementStatus), new object[] { element, isEnabled });
                return;
            }
            element.Enabled = isEnabled;
        }
    }
}
