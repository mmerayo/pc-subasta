using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Subasta.Extensions
{
	internal static class CrossThreadExtensions
	{
		public static void PerformSafely<T>(this T target, Action<Control> action) where T : Control
		{
			if (target.InvokeRequired)
			{
				target.Invoke(action, target);
			}
			else
			{
				action(target);
			}
		}


	}

	internal static class ControlExtensions
	{
		 public static IEnumerable<Control> FindControls(this Control root, Func<Control, bool> isMatch)
		{
			var matches = new List<Control>();

			Action<Control> filter = null;
			(filter = c =>
			          {
			          	if (isMatch(c))
			          		matches.Add(c);
			          	foreach (Control c2 in c.Controls)
			          		filter(c2);
			          })(root);

			return matches;
		}
	}
}
