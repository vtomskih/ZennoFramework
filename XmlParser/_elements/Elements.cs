/*-----------------------------------------------------
 Этот код был сгенерирован автоматически.
 Все изменения будут утеряны при следующей генерации.
 Дата: 29.04.2018 17:44:02 
 ------------------------------------------------------*/

using System.Collections.Generic;
using ZennoFramework;
using ZennoFramework.Infrastructure.Elements;
using ZennoFramework.Xml.Extensions;

namespace MyBotNamespace.Generated
{
    public partial class Elements : ElementContainer<BotContext>
    {
        public Elements(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
        {
            context.PrepareXPath(keysAndXpaths);
            Popups = new Popups(context, null);
            Wall = new Wall(context, null);
            LikeBtn = new ButtonsElements.WidgetsElements.LikeBtn(context);
            Items = new ElementsElements.Items(context);
            Parent1 = new ElementsElements.Parent1(context);
            Parent2 = new ElementsElements.Parent2(context);
            City = new ElementsElements.City(context);
            LoginPage = new ElementsElements.LoginPage(context);
        }
    
        public Popups Popups { get; }
        public Wall Wall { get; }
        public ButtonsElements.WidgetsElements.LikeBtn LikeBtn { get; }
        public ElementsElements.Items Items { get; }
        public ElementsElements.Parent1 Parent1 { get; }
        public ElementsElements.Parent2 Parent2 { get; }
        public ElementsElements.City City { get; }
    
        /// <summary>
        /// коммент 
        /// </summary>
        public ElementsElements.LoginPage LoginPage { get; }
    }

    namespace ElementsElements
    {
        public partial class Items : Element
        {
            private readonly Element _parent;
        
            public Items(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                LikeBtn = new ItemsElements.LikeBtn(context, parent);
                PostCollection = new ItemsElements.PostCollection<ItemsElements.PostItem>(context, parent);
                Item1 = new ItemsElements.Item1(context, parent);
                Item2 = new ItemsElements.Item2(context, parent);
            }
        
            public ItemsElements.LikeBtn LikeBtn { get; }
            public ItemsElements.PostCollection<ItemsElements.PostItem> PostCollection { get; }
            public ItemsElements.Item1 Item1 { get; }
            public ItemsElements.Item2 Item2 { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Elements.Items", _parent);
            }
        }

        namespace ItemsElements
        {
            public partial class LikeBtn : ButtonsElements.WidgetsElements.LikeBtn
            {
                private readonly Element _parent;
            
                public LikeBtn(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Items.Widgets.LikeBtn", _parent);
                }
            }

            public partial class PostCollection<TElement> : WallElements.PostCollection<TElement>
                where TElement: class
            {
                private readonly Element _parent;
            
            	public PostCollection(BotContext context, Element parent = null) : base(context)
            	{
                    _parent = parent;
            	}
                
            	protected override ElementCollection Find()
            	{
            		return Context.CreateCollection("Elements.Items.PostItem", _parent);
            	}
            }
            
            public partial class PostItem : WallElements.PostItem
            {
                private readonly Element _parent;
            
                public PostItem(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Items.PostItem", _parent);
                }
            }

            public partial class Item1 : Element
            {
                private readonly Element _parent;
            
                public Item1(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Items.Item1", _parent);
                }
            }

            public partial class Item2 : Element
            {
                private readonly Element _parent;
            
                public Item2(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Items.Item2", _parent);
                }
            }
        }

        public partial class Parent1 : Element
        {
            private readonly Element _parent;
        
            public Parent1(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                Child1 = new Parent1Elements.Child1(context, this);
                Child2 = new Parent1Elements.Child2(context);
                Child3 = new Parent1Elements.Child3(context, this);
                PostCollection = new Parent1Elements.PostCollection<Parent1Elements.PostItem>(context);
            }
        
            public Parent1Elements.Child1 Child1 { get; }
            public Parent1Elements.Child2 Child2 { get; }
            public Parent1Elements.Child3 Child3 { get; }
            public Parent1Elements.PostCollection<Parent1Elements.PostItem> PostCollection { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Elements.Parent1", _parent);
            }
        }

        namespace Parent1Elements
        {
            public partial class Child1 : Element
            {
                private readonly Element _parent;
            
                public Child1(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent1.Child1", _parent);
                }
            }

            public partial class Child2 : Element
            {
                public Child2(BotContext context) : base(context)
                {
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent1.Child2");
                }
            }

            public partial class Child3 : Element
            {
                private readonly Element _parent;
            
                public Child3(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent1.Child3", _parent);
                }
            }

            public partial class PostCollection<TElement> : WallElements.PostCollection<TElement>
                where TElement: class
            {
            	public PostCollection(BotContext context) : base(context)
            	{
            	}
                
            	protected override ElementCollection Find()
            	{
            		return Context.CreateCollection("Elements.Parent1.PostItem");
            	}
            }
            
            public partial class PostItem : WallElements.PostItem
            {
                private readonly Element _parent;
            
                public PostItem(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent1.PostItem", _parent);
                }
            }
        }

        public partial class Parent2 : Element
        {
            private readonly Element _parent;
        
            public Parent2(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                Child1 = new Parent2Elements.Child1(context, parent);
                Child2 = new Parent2Elements.Child2(context);
                Child3 = new Parent2Elements.Child3(context, parent);
            }
        
            public Parent2Elements.Child1 Child1 { get; }
            public Parent2Elements.Child2 Child2 { get; }
            public Parent2Elements.Child3 Child3 { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Elements.Parent2", _parent);
            }
        }

        namespace Parent2Elements
        {
            public partial class Child1 : Element
            {
                private readonly Element _parent;
            
                public Child1(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent2.Child1", _parent);
                }
            }

            public partial class Child2 : Element
            {
                public Child2(BotContext context) : base(context)
                {
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent2.Child2");
                }
            }

            public partial class Child3 : Element
            {
                private readonly Element _parent;
            
                public Child3(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.Parent2.Child3", _parent);
                }
            }
        }

        public partial class City : Element
        {
            private readonly Element _parent;
        
            public City(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                PostCollection = new CityElements.PostCollection<CityElements.PostItem>(context, parent);
                Wall = new Wall(context, null);
                TestItem1Collection = new CityElements.TestItem1Collection<CityElements.TestItem1>(context, parent);
                Moscow = new CityElements.Moscow(context, parent);
                London = new CityElements.London(context, parent);
            }
        
            public CityElements.PostCollection<CityElements.PostItem> PostCollection { get; }
            public Wall Wall { get; }
            public CityElements.TestItem1Collection<CityElements.TestItem1> TestItem1Collection { get; }
            public CityElements.Moscow Moscow { get; }
            public CityElements.London London { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Elements.City", _parent);
            }
        }

        namespace CityElements
        {
            public partial class PostCollection<TElement> : WallElements.PostCollection<TElement>
                where TElement: class
            {
                private readonly Element _parent;
            
            	public PostCollection(BotContext context, Element parent = null) : base(context)
            	{
                    _parent = parent;
            	}
                
            	protected override ElementCollection Find()
            	{
            		return Context.CreateCollection("Elements.City.PostItem", _parent);
            	}
            }
            
            public partial class PostItem : WallElements.PostItem
            {
                private readonly Element _parent;
            
                public PostItem(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.City.PostItem", _parent);
                }
            }

            public partial class Wall : ElementContainer<BotContext>
            {
                public Wall(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
                {
                }
            }

            public partial class TestItem1Collection<TElement> : ElementCollection<TElement>
                where TElement: class
            {
                private readonly Element _parent;
            
            	public TestItem1Collection(BotContext context, Element parent = null) : base(context)
            	{
                    _parent = parent;
            	}
                
            	protected override ElementCollection Find()
            	{
            		return Context.CreateCollection("Elements.City.TestItem1", _parent);
            	}
            }
            
            public partial class TestItem1 : Element
            {
                private readonly Element _parent;
            
                public TestItem1(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.City.TestItem1", _parent);
                }
            }

            public partial class Moscow : Element
            {
                private readonly Element _parent;
            
                public Moscow(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                    PostCollection = new WallElements.PostCollection<WallElements.PostItem>(context, this);
                    TestCollection = new MoscowElements.TestCollection<MoscowElements.TestItem>(context, this);
                    One = new MoscowElements.One(context, this);
                    Two = new MoscowElements.Two(context, this);
                }
            
                public WallElements.PostCollection<WallElements.PostItem> PostCollection { get; }
                public MoscowElements.TestCollection<MoscowElements.TestItem> TestCollection { get; }
                public MoscowElements.One One { get; }
                public MoscowElements.Two Two { get; }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.City.Moscow", _parent);
                }
            }

            namespace MoscowElements
            {
                public partial class TestCollection<TElement> : ElementCollection<TElement>
                    where TElement: class
                {
                    private readonly Element _parent;
                
                	public TestCollection(BotContext context, Element parent = null) : base(context)
                	{
                        _parent = parent;
                	}
                    
                	protected override ElementCollection Find()
                	{
                		return Context.CreateCollection("Elements.City.Moscow.TestItem", _parent);
                	}
                }
                
                public partial class TestItem : Element
                {
                    private readonly Element _parent;
                
                    public TestItem(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Elements.City.Moscow.TestItem", _parent);
                    }
                }

                public partial class One : Element
                {
                    private readonly Element _parent;
                
                    public One(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Elements.City.Moscow.One", _parent);
                    }
                }

                public partial class Two : Element
                {
                    private readonly Element _parent;
                
                    public Two(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                        PostCollection = new WallElements.PostCollection<WallElements.PostItem>(context, this);
                        A = new TwoElements.A(context, this);
                        B = new TwoElements.B(context);
                    }
                
                    public WallElements.PostCollection<WallElements.PostItem> PostCollection { get; }
                    public TwoElements.A A { get; }
                    public TwoElements.B B { get; }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Elements.City.Moscow.Two", _parent);
                    }
                }

                namespace TwoElements
                {
                    public partial class A : Element
                    {
                        private readonly Element _parent;
                    
                        public A(BotContext context, Element parent = null) : base(context)
                        {
                            _parent = parent;
                        }
                        
                        protected override Element Find()
                        {
                            return Context.CreateElement("Elements.City.Moscow.Two.A", _parent);
                        }
                    }

                    public partial class B : Element
                    {
                        public B(BotContext context) : base(context)
                        {
                        }
                        
                        protected override Element Find()
                        {
                            return Context.CreateElement("Elements.City.Moscow.Two.B");
                        }
                    }
                }
            }

            public partial class London : Element
            {
                private readonly Element _parent;
            
                public London(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                    One = new LondonElements.One(context, parent);
                    TwoCollection = new LondonElements.TwoCollection<LondonElements.Two>(context, parent);
                }
            
                public LondonElements.One One { get; }
                public LondonElements.TwoCollection<LondonElements.Two> TwoCollection { get; }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.City.London", _parent);
                }
            }

            namespace LondonElements
            {
                public partial class One : Element
                {
                    private readonly Element _parent;
                
                    public One(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Elements.City.London.One", _parent);
                    }
                }

                public partial class TwoCollection<TElement> : ElementCollection<TElement>
                    where TElement: class
                {
                    private readonly Element _parent;
                
                	public TwoCollection(BotContext context, Element parent = null) : base(context)
                	{
                        _parent = parent;
                	}
                    
                	protected override ElementCollection Find()
                	{
                		return Context.CreateCollection("Elements.City.London.Two", _parent);
                	}
                }
                
                public partial class Two : Element
                {
                    private readonly Element _parent;
                
                    public Two(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                        A = new TwoElements.A(context, this);
                        B = new TwoElements.B(context);
                        CCollection = new TwoElements.CCollection<TwoElements.C>(context, this);
                    }
                
                    public TwoElements.A A { get; }
                    public TwoElements.B B { get; }
                    public TwoElements.CCollection<TwoElements.C> CCollection { get; }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Elements.City.London.Two", _parent);
                    }
                }

                namespace TwoElements
                {
                    public partial class A : Element
                    {
                        private readonly Element _parent;
                    
                        public A(BotContext context, Element parent = null) : base(context)
                        {
                            _parent = parent;
                        }
                        
                        protected override Element Find()
                        {
                            return Context.CreateElement("Elements.City.London.Two.A", _parent);
                        }
                    }

                    public partial class B : Element
                    {
                        public B(BotContext context) : base(context)
                        {
                        }
                        
                        protected override Element Find()
                        {
                            return Context.CreateElement("Elements.City.London.Two.B");
                        }
                    }

                    public partial class CCollection<TElement> : ElementCollection<TElement>
                        where TElement: class
                    {
                        private readonly Element _parent;
                    
                    	public CCollection(BotContext context, Element parent = null) : base(context)
                    	{
                            _parent = parent;
                    	}
                        
                    	protected override ElementCollection Find()
                    	{
                    		return Context.CreateCollection("Elements.City.London.Two.C", _parent);
                    	}
                    }
                    
                    public partial class C : Element
                    {
                        private readonly Element _parent;
                    
                        public C(BotContext context, Element parent = null) : base(context)
                        {
                            _parent = parent;
                            Item1 = new CElements.Item1(context, this);
                            Item2 = new CElements.Item2(context);
                        }
                    
                        public CElements.Item1 Item1 { get; }
                        public CElements.Item2 Item2 { get; }
                        
                        protected override Element Find()
                        {
                            return Context.CreateElement("Elements.City.London.Two.C", _parent);
                        }
                    }

                    namespace CElements
                    {
                        public partial class Item1 : ElementContainer<BotContext>
                        {
                            public Item1(BotContext context, Element parent = null) : base(context)
                            {
                            }
                        }

                        public partial class Item2 : ElementContainer<BotContext>
                        {
                            public Item2(BotContext context) : base(context)
                            {
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// коммент 
        /// </summary>
        public partial class LoginPage : ElementContainer<BotContext>
        {
            public LoginPage(BotContext context, Element parent = null) : base(context)
            {
                LoginField = new LoginPageElements.LoginField(context, parent);
                PasswordField = new LoginPageElements.PasswordField(context, parent);
                LoginBtn = new LoginPageElements.LoginBtn(context, parent);
            }
        
            public LoginPageElements.LoginField LoginField { get; }
            public LoginPageElements.PasswordField PasswordField { get; }
            public LoginPageElements.LoginBtn LoginBtn { get; }
        }

        namespace LoginPageElements
        {
            public partial class LoginField : Element
            {
                private readonly Element _parent;
            
                public LoginField(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.LoginPage.LoginField", _parent);
                }
            }

            public partial class PasswordField : Element
            {
                private readonly Element _parent;
            
                public PasswordField(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.LoginPage.PasswordField", _parent);
                }
            }

            public partial class LoginBtn : Element
            {
                private readonly Element _parent;
            
                public LoginBtn(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Elements.LoginPage.LoginBtn", _parent);
                }
            }
        }
    }

    public partial class Popups : ElementContainer<BotContext>
    {
        public Popups(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
        {
            context.PrepareXPath(keysAndXpaths);
            PostPopup = new PopupsElements.PostPopup(context);
        }
    
        public PopupsElements.PostPopup PostPopup { get; }
    }

    namespace PopupsElements
    {
        public partial class PostPopup : Element
        {
            private readonly Element _parent;
        
            public PostPopup(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                Text = new PostPopupElements.Text(context, parent);
                LikeBtn = new PostPopupElements.LikeBtn(context, parent);
            }
        
            public PostPopupElements.Text Text { get; }
            public PostPopupElements.LikeBtn LikeBtn { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Popups.PostPopup", _parent);
            }
        }

        namespace PostPopupElements
        {
            public partial class Text : Element
            {
                private readonly Element _parent;
            
                public Text(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Popups.PostPopup.Text", _parent);
                }
            }

            public partial class LikeBtn : ButtonsElements.WidgetsElements.LikeBtn
            {
                private readonly Element _parent;
            
                public LikeBtn(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Popups.PostPopup.Widgets.LikeBtn", _parent);
                }
            }
        }
    }

    public partial class Buttons : ElementContainer<BotContext>
    {
        public Buttons(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
        {
            context.PrepareXPath(keysAndXpaths);
            LikeBtn = new ButtonsElements.LikeBtn(context);
            RepostBtn = new ButtonsElements.RepostBtn(context);
            Widgets = new ButtonsElements.Widgets(context);
        }
    
        public ButtonsElements.LikeBtn LikeBtn { get; }
        public ButtonsElements.RepostBtn RepostBtn { get; }
        public ButtonsElements.Widgets Widgets { get; }
    }

    namespace ButtonsElements
    {
        public partial class LikeBtn : Element
        {
            private readonly Element _parent;
        
            public LikeBtn(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
            }
            
            protected override Element Find()
            {
                return Context.CreateElement("Buttons.LikeBtn", _parent);
            }
        }

        public partial class RepostBtn : Element
        {
            private readonly Element _parent;
        
            public RepostBtn(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
            }
            
            protected override Element Find()
            {
                return Context.CreateElement("Buttons.RepostBtn", _parent);
            }
        }

        public partial class Widgets : Element
        {
            private readonly Element _parent;
        
            public Widgets(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                LikeBtn = new WidgetsElements.LikeBtn(context, parent);
                RepostBtn = new WidgetsElements.RepostBtn(context, parent);
                CommentBtn = new WidgetsElements.CommentBtn(context, parent);
            }
        
            public WidgetsElements.LikeBtn LikeBtn { get; }
            public WidgetsElements.RepostBtn RepostBtn { get; }
            public WidgetsElements.CommentBtn CommentBtn { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Buttons.Widgets", _parent);
            }
        }

        namespace WidgetsElements
        {
            public partial class LikeBtn : ButtonsElements.LikeBtn
            {
                private readonly Element _parent;
            
                public LikeBtn(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Buttons.Widgets.LikeBtn", _parent);
                }
            }

            public partial class RepostBtn : ButtonsElements.RepostBtn
            {
                private readonly Element _parent;
            
                public RepostBtn(BotContext context, Element parent = null) : base(context, parent)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Buttons.Widgets.RepostBtn", _parent);
                }
            }

            public partial class CommentBtn : Element
            {
                private readonly Element _parent;
            
                public CommentBtn(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Buttons.Widgets.CommentBtn", _parent);
                }
            }
        }
    }

    public partial class Wall : ElementContainer<BotContext>
    {
        public Wall(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
        {
            context.PrepareXPath(keysAndXpaths);
            PostCollection = new WallElements.PostCollection<WallElements.PostItem>(context);
        }
    
        public WallElements.PostCollection<WallElements.PostItem> PostCollection { get; }
    }

    namespace WallElements
    {
        public partial class PostCollection<TElement> : ElementCollection<TElement>
            where TElement: class
        {
            private readonly Element _parent;
        
        	public PostCollection(BotContext context, Element parent = null) : base(context)
        	{
                _parent = parent;
        	}
            
        	protected override ElementCollection Find()
        	{
        		return Context.CreateCollection("Wall.PostItem", _parent);
        	}
        }
        
        public partial class PostItem : Element
        {
            private readonly Element _parent;
        
            public PostItem(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                Text = new PostItemElements.Text(context, this);
                LikeBtn = new ButtonsElements.WidgetsElements.LikeBtn(context, this);
                CommentBlock = new CommentsElements.CommentBlock(context, this);
            }
        
            public PostItemElements.Text Text { get; }
            public ButtonsElements.WidgetsElements.LikeBtn LikeBtn { get; }
            public CommentsElements.CommentBlock CommentBlock { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Wall.PostItem", _parent);
            }
        }

        namespace PostItemElements
        {
            public partial class Text : Element
            {
                private readonly Element _parent;
            
                public Text(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Wall.PostItem.Text", _parent);
                }
            }


        }
    }

    public partial class Comments : ElementContainer<BotContext>
    {
        public Comments(BotContext context, Dictionary<string, string> keysAndXpaths) : base(context)
        {
            context.PrepareXPath(keysAndXpaths);
            CommentBlock = new CommentsElements.CommentBlock(context);
        }
    
        public CommentsElements.CommentBlock CommentBlock { get; }
    }

    namespace CommentsElements
    {
        public partial class CommentBlock : Element
        {
            private readonly Element _parent;
        
            public CommentBlock(BotContext context, Element parent = null) : base(context)
            {
                _parent = parent;
                CommentCollection = new CommentBlockElements.CommentCollection<CommentBlockElements.CommentItem>(context, parent);
                AddComment = new CommentBlockElements.AddComment(context, parent);
            }
        
            public CommentBlockElements.CommentCollection<CommentBlockElements.CommentItem> CommentCollection { get; }
            public CommentBlockElements.AddComment AddComment { get; }
            
            protected override Element Find()
            {
                return Context.CreateElement("Comments.CommentBlock", _parent);
            }
        }

        namespace CommentBlockElements
        {
            public partial class CommentCollection<TElement> : ElementCollection<TElement>
                where TElement: class
            {
                private readonly Element _parent;
            
            	public CommentCollection(BotContext context, Element parent = null) : base(context)
            	{
                    _parent = parent;
            	}
                
            	protected override ElementCollection Find()
            	{
            		return Context.CreateCollection("Comments.CommentBlock.CommentItem", _parent);
            	}
            }
            
            public partial class CommentItem : Element
            {
                private readonly Element _parent;
            
                public CommentItem(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                    Message = new CommentItemElements.Message(context, this);
                    ReplyBtn = new CommentItemElements.ReplyBtn(context, this);
                    LikeBtn = new ButtonsElements.LikeBtn(context, this);
                }
            
                public CommentItemElements.Message Message { get; }
                public CommentItemElements.ReplyBtn ReplyBtn { get; }
                public ButtonsElements.LikeBtn LikeBtn { get; }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Comments.CommentBlock.CommentItem", _parent);
                }
            }

            namespace CommentItemElements
            {
                public partial class Message : Element
                {
                    private readonly Element _parent;
                
                    public Message(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Comments.CommentBlock.CommentItem.Message", _parent);
                    }
                }

                public partial class ReplyBtn : Element
                {
                    private readonly Element _parent;
                
                    public ReplyBtn(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Comments.CommentBlock.CommentItem.ReplyBtn", _parent);
                    }
                }

            }

            public partial class AddComment : Element
            {
                private readonly Element _parent;
            
                public AddComment(BotContext context, Element parent = null) : base(context)
                {
                    _parent = parent;
                    MessageField = new AddCommentElements.MessageField(context, parent);
                    SendCommentBtn = new AddCommentElements.SendCommentBtn(context, parent);
                }
            
                public AddCommentElements.MessageField MessageField { get; }
                public AddCommentElements.SendCommentBtn SendCommentBtn { get; }
                
                protected override Element Find()
                {
                    return Context.CreateElement("Comments.CommentBlock.AddComment", _parent);
                }
            }

            namespace AddCommentElements
            {
                public partial class MessageField : Element
                {
                    private readonly Element _parent;
                
                    public MessageField(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Comments.CommentBlock.AddComment.MessageField", _parent);
                    }
                }

                public partial class SendCommentBtn : Element
                {
                    private readonly Element _parent;
                
                    public SendCommentBtn(BotContext context, Element parent = null) : base(context)
                    {
                        _parent = parent;
                    }
                    
                    protected override Element Find()
                    {
                        return Context.CreateElement("Comments.CommentBlock.AddComment.SendCommentBtn", _parent);
                    }
                }
            }
        }
    }
}
