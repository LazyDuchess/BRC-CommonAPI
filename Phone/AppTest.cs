using Reptile;
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
            ScrollView = PhoneScrollView.Create(this);

            for(var i = 0; i < 10; i++) {
                var button = PhoneUIUtility.CreateSimpleButton("This is a test button to test things.");
                button.OnConfirm += () => {
                    MyPhone.OpenApp(typeof(Reptile.Phone.AppCamera));
                };
                ScrollView.AddButton(button);
            }
        }
    }
}
