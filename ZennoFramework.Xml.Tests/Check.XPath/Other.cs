using NUnit.Framework;
using ZennoFramework.Generation.Tests;

namespace ZennoFramework.Xml.Tests.Check.XPath
{
    [TestFixture]
    public class Other
    {
        [Test]
        public void Popups()
        {
            var doc = Config.Docs[2];

            Assert.AreEqual(doc.GetXPath("PostPopup"), "//PostPopup");
            Assert.AreEqual(doc.GetXPath("PostPopup.Text"), "//PostPopup//Text");
            Assert.AreEqual(doc.GetXPath("PostPopup.Widgets.LikeBtn"), "//PostPopup//Widgets//LikeBtn");
        }

        [Test]
        public void Buttons()
        {
            var doc = Config.Docs[3];

            Assert.AreEqual(doc.GetXPath("LikeBtn"), "//LikeBtn");
            Assert.AreEqual(doc.GetXPath("RepostBtn"), "//RepostBtn");
            Assert.AreEqual(doc.GetXPath("Widgets"), "//Widgets");
            Assert.AreEqual(doc.GetXPath("Widgets.LikeBtn"), "//Widgets//LikeBtn");
            Assert.AreEqual(doc.GetXPath("Widgets.RepostBtn"), "//Widgets//RepostBtn");
            Assert.AreEqual(doc.GetXPath("Widgets.CommentBtn"), "//Widgets//CommentBtn");
        }

        [Test]
        public void Wall()
        {
            var doc = Config.Docs[4];

            Assert.AreEqual(doc.GetXPath("PostItem"), "//PostItem");
            Assert.AreEqual(doc.GetXPath("PostItem.Text"), "//Text");
            Assert.AreEqual(doc.GetXPath("PostItem.Widgets.LikeBtn"), "//Widgets//LikeBtn");
            Assert.AreEqual(doc.GetXPath("PostItem.CommentBlock"), "//CommentBlock");
            //Assert.IsEmpty(doc.GetXPath("PostItem.Widgets.LikeBtn"));
            //Assert.IsEmpty(doc.GetXPath("PostItem.CommentBlock"));
        }

        [Test]
        public void Comments()
        {
            var doc = Config.Docs[5];

            Assert.AreEqual(doc.GetXPath("CommentBlock"), "//CommentBlock");
            Assert.AreEqual(doc.GetXPath("CommentBlock.CommentItem"), "//CommentBlock//CommentItem");
            Assert.AreEqual(doc.GetXPath("CommentBlock.CommentItem.Message"), "//Message");
            Assert.AreEqual(doc.GetXPath("CommentBlock.CommentItem.ReplyBtn"), "//ReplyBtn");
            Assert.AreEqual(doc.GetXPath("CommentBlock.CommentItem.LikeBtn"), "//LikeBtn");
            Assert.AreEqual(doc.GetXPath("CommentBlock.AddComment"), "//CommentBlock//AddComment");
            Assert.AreEqual(doc.GetXPath("CommentBlock.AddComment.MessageField"),
                "//CommentBlock//AddComment//MessageField");
            Assert.AreEqual(doc.GetXPath("CommentBlock.AddComment.SendCommentBtn"),
                "//CommentBlock//AddComment//SendCommentBtn");
        }
    }
}