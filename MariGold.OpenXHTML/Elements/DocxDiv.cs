﻿namespace MariGold.OpenXHTML
{
	using System;
	using MariGold.HtmlParser;
	using DocumentFormat.OpenXml;
	using DocumentFormat.OpenXml.Wordprocessing;
	
	internal sealed class DocxDiv : WordElement
	{
		internal DocxDiv(IOpenXmlContext context)
			: base(context)
		{
		}
		
		internal override bool IsBlockLine
		{
			get
			{
				return true;
			}
		}
		
		internal override bool CanConvert(HtmlNode node)
		{
			return string.Compare(node.Tag, "div", true) == 0;
		}
		
		internal override void Process(HtmlNode node, OpenXmlElement parent)
		{
			if (node != null && parent != null)
			{
				OpenXmlElement paragraph = parent.AppendChild(new Paragraph());
				Run run = null;
				
				foreach (HtmlNode child in node.Children)
				{
					if (child.Tag == "#text")
					{
						if (run == null)
						{
							run = paragraph.AppendChild(new Run());
						}
						
						run.AppendChild(new Text(node.InnerHtml));
					}
					else
					{
						run = null;
						
						ProcessChild(child, paragraph);
					}
				}
			}
		}
	}
}
