﻿namespace MariGold.OpenXHTML
{
    using System;
    using MariGold.HtmlParser;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;

    internal sealed class DocxUnderline : DocxElement, ITextElement
    {
        private void SetStyle(DocxNode node)
        {
            string value = node.ExtractStyleValue(DocxFont.underLine);

            if (string.IsNullOrEmpty(value))
            {
                node.SetExtentedStyle(DocxFont.textDecoration, DocxFont.underLine);
            }
        }

        internal DocxUnderline(IOpenXmlContext context)
            : base(context)
        {
        }

        internal override bool CanConvert(DocxNode node)
        {
            return string.Compare(node.Tag, "u", StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        internal override void Process(DocxNode node, ref Paragraph paragraph)
        {
            if (node.IsNull() || IsHidden(node))
            {
                return;
            }

            SetStyle(node);

            foreach (DocxNode child in node.Children)
            {
                if (child.IsText)
                {
                    if (!IsEmptyText(child.InnerHtml))
                    {
                        if (paragraph == null)
                        {
                            paragraph = node.Parent.AppendChild(new Paragraph());
                            ParagraphCreated(node.ParagraphNode, paragraph);
                        }

                        Run run = paragraph.AppendChild(new Run(new Text()
                        {
                            Text = ClearHtml(child.InnerHtml),
                            Space = SpaceProcessingModeValues.Preserve
                        }));

                        RunCreated(node, run);
                    }
                }
                else
                {
                    child.ParagraphNode = node.ParagraphNode;
                    child.Parent = node.Parent;
                    node.CopyExtentedStyles(child);
                    ProcessChild(child, ref paragraph);
                }
            }
        }

        bool ITextElement.CanConvert(DocxNode node)
        {
            return CanConvert(node);
        }

        void ITextElement.Process(DocxNode node)
        {
            if (IsHidden(node))
            {
                return;
            }

            SetStyle(node);
            ProcessTextChild(node);
        }
    }
}
