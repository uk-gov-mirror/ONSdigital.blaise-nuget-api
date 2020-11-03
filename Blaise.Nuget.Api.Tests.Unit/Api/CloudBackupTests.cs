using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class CloudBackupTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly string _filePath;
        private readonly string _bucketName;
        private readonly string _folderName;

        private IBlaiseApi _sut;

        public CloudBackupTests()
        {
            _filePath = "filePath";
            _bucketName = "OpnBucket";
            _folderName = "FolderName";
        }
        [SetUp]
        public void SetUpTests()
        {
            _caseServiceMock = new Mock<ICaseService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _sut = new BlaiseApi(
                _caseServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_BackupFilesToBucket_The_Correct_Services_Are_Called()
        {
            //arrange
            var fileList = new List<string>
            {
                "file1.bdix",
                "file1.bmix",
                "file1.bdbx"
            };

            _fileServiceMock.Setup(f => f.GetFiles(It.IsAny<string>()))
                .Returns(fileList);

            _fileServiceMock.Setup(f => f.UploadFilesToBucket(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.BackupFilesToBucket(_filePath, _bucketName, _folderName);

            //assert
            foreach (var file in fileList)
            {
                _fileServiceMock.Verify(v => 
                    v.UploadFilesToBucket(file, _bucketName, _folderName), Times.Once);

            }
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => 
                _sut.BackupFilesToBucket(string.Empty, _bucketName, _folderName));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => 
                _sut.BackupFilesToBucket(null, _bucketName, _folderName));
            Assert.AreEqual("filePath", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_BucketName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => 
                _sut.BackupFilesToBucket(_filePath, string.Empty, _folderName));
            Assert.AreEqual("A value for the argument 'bucketName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_BucketName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                _sut.BackupFilesToBucket(_filePath, null, _folderName));
            Assert.AreEqual("bucketName", exception.ParamName);
        }
    }
}
