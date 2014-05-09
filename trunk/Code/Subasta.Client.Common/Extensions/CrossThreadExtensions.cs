using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Subasta.Client.Common.Extensions
{
	public static class CrossThreadExtensions
	{
		public static void PerformSafely<T>(this T target, Action<T> action) where T : Control
		{
			if (target.InvokeRequired)
			{
				target.Invoke(action, target);
				Application.DoEvents();
			}
			else
			{
				action(target);
			}
		}


	}

	public static class ControlExtensions
	{
		public static IEnumerable<TControl> FindControls<TControl>(this Control root, Func<Control, bool> isMatch)
		{
			var matches = new List<Control>();

			Action<Control> filter = null;
			(filter = c =>
			          {
			          	if (c is TControl && isMatch(c))
			          		matches.Add(c);
			          	foreach (Control c2 in c.Controls)
			          		filter(c2);
			          })(root);

			return matches.Cast<TControl>();
		}

		public static IEnumerable<TControl> FindControls<TControl>(this Control root)
		{
			return root.FindControls<TControl>((c) => true);
		}

		public static TControl FindControl<TControl>(this Control root, string name)
		{
			return root.FindControls<TControl>((c) => c.Name==name).SingleOrDefault();
		}
	}
}
