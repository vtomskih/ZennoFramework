using NUnit.Framework;
using ZennoFramework.Generation.Tests;

namespace ZennoFramework.Xml.Tests.Check.Keys
{
    [TestFixture]
    public class Other
    {
        [Test]
        public void Elements()
        {
            var doc = Config.Docs[0];

            Assert.AreEqual(doc.GetKey("Popups"), "Popups");
            Assert.AreEqual(doc.GetImportedKey("Popups"), "Popups");
            Assert.AreEqual(doc.GetKey("Wall"), "Wall");
            Assert.AreEqual(doc.GetImportedKey("Wall"), "Wall");
        }

        [Test]
        public void Popups()
        {
            var doc = Config.Docs[2];

            Assert.AreEqual(doc.GetKey("PostPopup"), "Popups.PostPopup");
            Assert.AreEqual(doc.GetKey("PostPopup.Text"), "Popups.PostPopup.Text");
            Assert.AreEqual(doc.GetKey("PostPopup.Widgets.LikeBtn"), "Popups.PostPopup.Widgets.LikeBtn");
        }

        [Test]
        public void Buttons()
        {
            var doc = Config.Docs[3];

            Assert.AreEqual(doc.GetKey("LikeBtn"), "Buttons.LikeBtn");
            Assert.AreEqual(doc.GetKey("RepostBtn"), "Buttons.RepostBtn");
            Assert.AreEqual(doc.GetKey("Widgets"), "Buttons.Widgets");
            Assert.AreEqual(doc.GetKey("Widgets.LikeBtn"), "Buttons.Widgets.LikeBtn");
            Assert.AreEqual(doc.GetKey("Widgets.RepostBtn"), "Buttons.Widgets.RepostBtn");
            Assert.AreEqual(doc.GetKey("Widgets.CommentBtn"), "Buttons.Widgets.CommentBtn");
        }

        [Test]
        public void Wall()
        {
            var doc = Config.Docs[4];

            Assert.AreEqual(doc.GetKey("PostItem"), "Wall.PostItem");
            Assert.AreEqual(doc.GetKey("PostItem.Text"), "Wall.PostItem.Text");
            Assert.AreEqual(doc.GetKey("PostItem.Widgets.LikeBtn"), "Buttons.Widgets.LikeBtn");
            Assert.AreEqual(doc.GetKey("PostItem.CommentBlock"), "Comments.CommentBlock");
            Assert.AreEqual(doc.GetImportedKey("PostItem.CommentBlock"), "Comments.CommentBlock");
        }

        [Test]
        public void Comments()
        {
            var doc = Config.Docs[5];

            Assert.AreEqual(doc.GetKey("CommentBlock"), "Comments.CommentBlock");
            Assert.AreEqual(doc.GetKey("CommentBlock.CommentItem"), "Comments.CommentBlock.CommentItem");
            Assert.AreEqual(doc.GetKey("CommentBlock.CommentItem.Message"),
                "Comments.CommentBlock.CommentItem.Message");
            Assert.AreEqual(doc.GetKey("CommentBlock.CommentItem.ReplyBtn"),
                "Comments.CommentBlock.CommentItem.ReplyBtn");
            Assert.AreEqual(doc.GetKey("CommentBlock.CommentItem.LikeBtn"), "Buttons.LikeBtn");
            Assert.AreEqual(doc.GetKey("CommentBlock.AddComment"), "Comments.CommentBlock.AddComment");
            Assert.AreEqual(doc.GetKey("CommentBlock.AddComment.MessageField"),
                "Comments.CommentBlock.AddComment.MessageField");
            Assert.AreEqual(doc.GetKey("CommentBlock.AddComment.SendCommentBtn"),
                "Comments.CommentBlock.AddComment.SendCommentBtn");
        }
    }
}