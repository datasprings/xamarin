using System;
using AppKit;

namespace Xamarin.Forms
{
	[AttributeUsage (AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ExportRendererAttribute : HandlerAttribute {
		internal bool Idiomatic { get; private set; }


		public ExportRendererAttribute (Type handler, Type target)
			: base (handler, target) {
			Idiomatic = false;
			}

		public override bool ShouldRegister () {
			return !Idiomatic;
		}
	}
}