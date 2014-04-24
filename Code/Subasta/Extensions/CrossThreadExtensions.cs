using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Subasta.Extensions
{
	static class CrossThreadExtensions
	{


		delegate void ControlShowSafeCallback(Form form);
		public static void ControlShowSafe(Form form)
		{
			if (form.InvokeRequired)
			{
				var d = new ControlShowSafeCallback(ControlShowSafe);
				form.Invoke(d, new object[] { form});
			}
			else
			{
				form.Show();
			}
		}

		public static void ShowSafe(this Form form)
		{
			ControlShowSafe(form);
		}

	}
}
