using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Subasta.Extensions
{
	static class CrossThreadExtensions
	{
		public static void PerformSafely<T>(this T target, Action<Control> action) where T:Control
		{
			if (target.InvokeRequired)
			{
				target.Invoke(action,target);
			}
			else
			{
				action(target);
			}
		}

		
	}
}
