using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI.Phone {
    public class AppTest : CustomApp {
        public override void OnAppInit() {
            base.OnAppInit();
            CreateIconlessTitleBar("Test App");
            var scrollview = PhoneScrollView.Create(this);
            var button = SimplePhoneButton.Create();
            scrollview.AddButton(button);
        }
    }
}
