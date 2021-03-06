﻿namespace MariGold.OpenXHTML
{
    using System;
    using MariGold.HtmlParser;
    using System.Collections.Generic;
    using DocumentFormat.OpenXml;

    internal sealed class DocxNode
    {
        private readonly IHtmlNode node;

        private DocxNode paragraphNode;
        private OpenXmlElement parent;
        private Dictionary<string, string> extentedStyles;
        private Dictionary<string, string> styles;

        private void SetExtentedStyles(Dictionary<string, string> extentedStyles)
        {
            this.extentedStyles = new Dictionary<string,string>();

            foreach(var style in extentedStyles)
            {
                if (!node.Styles.ContainsKey(style.Key))
                {
                    this.extentedStyles.Add(style.Key, style.Value);
                }
            }
        }

        internal string Tag
        {
            get
            {
                return node.Tag;
            }
        }

        internal string Html
        {
            get
            {
                return node.Html;
            }
        }

        internal string InnerHtml
        {
            get
            {
                return node.InnerHtml;
            }
        }

        internal bool IsText
        {
            get
            {
                return node.IsText;
            }
        }

        internal DocxNode Next
        {
            get
            {
                if (node.Next == null)
                {
                    return null;
                }

                return new DocxNode(node.Next);
            }
        }

        internal DocxNode Previous
        {
            get
            {
                if (node.Previous == null)
                {
                    return null;
                }

                return new DocxNode(node.Previous);
            }
        }

        internal bool HasChildren
        {
            get
            {
                return node.HasChildren;
            }
        }

        internal IEnumerable<DocxNode> Children
        {
            get
            {
                foreach (var child in node.Children)
                {
                    yield return new DocxNode(child);
                }
            }
        }
       
        internal DocxNode ParagraphNode
        {
            get
            {
                return paragraphNode ?? this;
            }

            set
            {
                paragraphNode = value;
            }
        }

        internal Dictionary<string, string> Styles
        {
            get
            {
                return styles;
            }
        }

        internal OpenXmlElement Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        internal DocxNode(IHtmlNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            this.node = node;
            this.styles = node.Styles;
            this.extentedStyles = new Dictionary<string, string>();
        }

        internal bool IsNull()
        {
            return node == null;
        }

        internal string ExtractAttributeValue(string attributeName)
        {
            foreach (KeyValuePair<string, string> attribute in node.Attributes)
            {
                if (string.Compare(attributeName, attribute.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return attribute.Value;
                }
            }

            return string.Empty;
        }

        internal string ExtractStyleValue(string styleName)
        {
            foreach (KeyValuePair<string, string> style in styles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            foreach (KeyValuePair<string, string> style in extentedStyles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            foreach (KeyValuePair<string, string> style in node.InheritedStyles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            return string.Empty;
        }

        internal string ExtractOwnStyleValue(string styleName)
        {
            foreach (KeyValuePair<string, string> style in styles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            return string.Empty;
        }

        internal string ExtractInheritedStyleValue(string styleName)
        {
            foreach (KeyValuePair<string, string> style in extentedStyles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            foreach (KeyValuePair<string, string> style in node.InheritedStyles)
            {
                if (string.Compare(styleName, style.Key, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return style.Value;
                }
            }

            return string.Empty;
        }

        internal void SetExtentedStyle(string styleName, string value)
        {
            this.extentedStyles[styleName] = value;
        }

        internal void CopyExtentedStyles(DocxNode toNode)
        {
            if (toNode.IsNull())
            {
                return;
            }

            toNode.SetExtentedStyles(this.extentedStyles);
        }

        internal void RemoveStyles(params string[] styleNames)
        {
            foreach (string styleName in styleNames)
            {
                styles.Remove(styleName);
            }
        }
    }
}
