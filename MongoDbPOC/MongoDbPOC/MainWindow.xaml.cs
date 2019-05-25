using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbPOC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MongoDbPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MainAsync().Wait();
            FeedDataToDb();
        }

        public void FeedDataToDb()
        {
            var client = ConnectToMongoDb();
            IMongoDatabase db = client.GetDatabase("LASXdb");
            //IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("ExperimentFolderDetails");
            CreateNewLASXRandomData();

        }
        public MongoClient ConnectToMongoDb()
        {
            var client = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            return client;
        }


        static async Task MainAsync()
        {

            var client = new MongoClient();

            IMongoDatabase db = client.GetDatabase("LASXdb");

            var collection = db.GetCollection<Student>("students");
            var newStudents = CreateNewStudents();

            await collection.InsertManyAsync(newStudents);
        }

        private static IEnumerable<Student> CreateNewStudents()
        {
            var student1 = new Student
            {
                FirstName = "Gregor",
                LastName = "Felix",
                Subjects = new List<string>() { "English", "Mathematics", "Physics", "Biology" },
                Class = "JSS 3",
                Age = 23
            };

            var student2 = new Student
            {
                FirstName = "Machiko",
                LastName = "Elkberg",
                Subjects = new List<string> { "English", "Mathematics", "Spanish" },
                Class = "JSS 3",
                Age = 23
            };

            var student3 = new Student
            {
                FirstName = "Julie",
                LastName = "Sandal",
                Subjects = new List<string> { "English", "Mathematics", "Physics", "Chemistry" },
                Class = "JSS 1",
                Age = 25
            };

            var newStudents = new List<Student> { student1, student2, student3 };

            return newStudents;
        }


        public async void CreateNewLASXRandomData()
        {
            //COLLECTION SCHEMA
            List<ExperimentDataset> dtsetCollection = new List<ExperimentDataset>();
            List<UserDefinedFields> lstFieldsList = new List<UserDefinedFields>();
            List<UserDetails> lstUsrDetails = new List<UserDetails>();
            List<Templates> lstTemplates = new List<Templates>();
            List<GroupDetails> lstGrpDetails = new List<GroupDetails>();
            List<SoftwareDetails> lstSwftDetails = new List<SoftwareDetails>();
            List<HistoryDetails> lstHistoryDetails = new List<HistoryDetails>();
            List<ExperimentFolderDetails> lstExperimentFolderDetails = new List<ExperimentFolderDetails>();

            #region CREATE MOCK DATA 
            UserDetails usrDetails = new UserDetails();
            usrDetails.userId = 1;
            usrDetails.userName = "Shashank";
            lstUsrDetails.Add(usrDetails);

            UserDefinedFields usrDefFields = new UserDefinedFields();
            usrDefFields.udfId = 1;
            usrDefFields.createdBy = usrDetails;
            usrDefFields.createdTimeStamp = DateTime.Now;
            usrDefFields.fieldDataType = "TEXT";
            usrDefFields.fieldName = "TEST_FIELD";
            usrDefFields.fieldValue = "TEST_VALUE";
            usrDefFields.isActive = true;

            UserDefinedFields usrDefFields1 = new UserDefinedFields();
            usrDefFields1.udfId = 1;
            usrDefFields1.createdBy = usrDetails;
            usrDefFields1.createdTimeStamp = DateTime.Now;
            usrDefFields1.fieldDataType = "TEXT1";
            usrDefFields1.fieldName = "TEST_FIELD1";
            usrDefFields1.fieldValue = "TEST_VALUE1";
            usrDefFields1.isActive = true;

            UserDefinedFields usrDefFields2 = new UserDefinedFields();
            usrDefFields2.udfId = 1;
            usrDefFields2.createdBy = usrDetails;
            usrDefFields2.createdTimeStamp = DateTime.Now;
            usrDefFields2.fieldDataType = "TEXT2";
            usrDefFields2.fieldName = "TEST_FIELD2";
            usrDefFields2.fieldValue = "TEST_VALUE2";
            usrDefFields2.isActive = true;

            UserDefinedFields usrDefFields3 = new UserDefinedFields();
            usrDefFields3.udfId = 1;
            usrDefFields3.createdBy = usrDetails;
            usrDefFields3.createdTimeStamp = DateTime.Now;
            usrDefFields3.fieldDataType = "TEXT3";
            usrDefFields3.fieldName = "TEST_FIELD3";
            usrDefFields3.fieldValue = "TEST_VALUE3";
            usrDefFields3.isActive = true;

            lstFieldsList.Add(usrDefFields);
            lstFieldsList.Add(usrDefFields1);
            lstFieldsList.Add(usrDefFields2);
            lstFieldsList.Add(usrDefFields3);

            GroupDetails grd = new GroupDetails();
            grd.groupId = 1;
            grd.groupName = "Admin";
            lstGrpDetails.Add(grd);

            Templates tem = new Templates();
            tem.templateId = 1;
            tem.templateGroup = grd;
            tem.status = "ACTIVE";
            tem.type = "PERSONAL";
            tem.createdBy = usrDetails;
            tem.createdTimeStamp = DateTime.Now;
            tem.fields = lstFieldsList;
            lstTemplates.Add(tem);

            SoftwareDetails swfDtl = new SoftwareDetails();
            swfDtl.compatibleSoftwareId = 1;
            swfDtl.compatibleSoftwareName = "LASX";
            swfDtl.manufacturerId = 2;
            swfDtl.manufacturerName = "LASX_MANF";
            swfDtl.OriginalSoftwareName = "LASX";
            swfDtl.OriginalSoftwareVersion = "4.0.0.1";
            swfDtl.softwareDetailsId = 1;
            lstSwftDetails.Add(swfDtl);

            HistoryDetails hstDetails = new HistoryDetails();
            hstDetails.action = "LOADED";
            hstDetails.createdTimeStamp = DateTime.Now;
            hstDetails.historyId = 1;
            hstDetails.isActive = true;
            hstDetails.userDetails = usrDetails;
            lstHistoryDetails.Add(hstDetails);

            ExperimentFolderDetails expFldrDetails = new ExperimentFolderDetails();
            expFldrDetails.createdTimeStamp = DateTime.Now;
            expFldrDetails.experimentFolderId = 1;
            expFldrDetails.experimentFolderName = "TEST_FOLDER";
            expFldrDetails.folderSize = "789340002";
            expFldrDetails.path = "C://TESTEXP/";
            expFldrDetails.status = "ACTIVE";
            lstExperimentFolderDetails.Add(expFldrDetails);

            ExperimentDataset expDtSet = new ExperimentDataset();
            expDtSet.createdTimeStamp = DateTime.Now;
            expDtSet.groupId = grd;
            expDtSet.historyId = hstDetails;
            expDtSet.softwareDetailsId = swfDtl;
            expDtSet.status = "ENABLED";
            expDtSet.uniqueId = "EXP78902738902";
            expDtSet.userDefinedFieldsId = usrDefFields;
            expDtSet.userDetail = usrDetails;

            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);
            dtsetCollection.Add(expDtSet);

            #endregion

            var client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("LASXdb");

            var collectionExperimentDataset = db.GetCollection<ExperimentDataset>("ExperimentDataset");
            var collectionExperimentFolderDetails = db.GetCollection<ExperimentFolderDetails>("ExperimentFolderDetails");
            var collectionGroupDetails = db.GetCollection<GroupDetails>("GroupDetails");
            var collectionHistoryDetails = db.GetCollection<HistoryDetails>("HistoryDetails");
            var collectionSoftwareDetails = db.GetCollection<SoftwareDetails>("SoftwareDetails");
            var collectionTemplates = db.GetCollection<Templates>("Templates");
            var collectionUserDefinedFields = db.GetCollection<UserDefinedFields>("UserDefinedFields");
            var collectionUserDetails = db.GetCollection<UserDetails>("UserDetails");


           

            await collectionExperimentFolderDetails.InsertManyAsync(lstExperimentFolderDetails);
            await collectionGroupDetails.InsertManyAsync(lstGrpDetails);
            await collectionHistoryDetails.InsertManyAsync(lstHistoryDetails);
            await collectionSoftwareDetails.InsertManyAsync(lstSwftDetails);
            await collectionTemplates.InsertManyAsync(lstTemplates);
            await collectionUserDefinedFields.InsertManyAsync(lstFieldsList);
            await collectionUserDetails.InsertManyAsync(lstUsrDetails);
            await collectionExperimentDataset.InsertManyAsync(dtsetCollection);

        }


    }

}

