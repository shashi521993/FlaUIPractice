using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Capturing;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.Core.WindowsAPI;
using FlaUI.UIA2;
using FlaUI.UIA3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace FlaUiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void newUserRegistration()
        {

           


            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankSystem.exe");
            var application = FlaUI.Core.Application.Launch(exePath);

            //var application = FlaUI.Core.Application.Launch(@"D:\GitClonedRepo\FlaUIPractice-master\FlaUIPractice-master\FlaUIPractice\BankSystem\bin\Release\BankSystem.exe");
            var automation = new UIA3Automation();
            var mainWindow = application.GetMainWindow(automation);
            ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

            mainWindow.FindFirstDescendant(cf.ByName("Registration")).AsButton().Click();
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InFName")).AsTextBox().Enter("Marissa");
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InLName")).AsTextBox().Enter("Santos");
            //Thread.Sleep(500);
            var age = mainWindow.FindFirstDescendant(cf.ByAutomationId("InAge")).AsComboBox();
            if (age != null)
            {
                age.Patterns.ExpandCollapse.Pattern.Expand();
                age.Select("34");
            }
            var country = mainWindow.FindFirstDescendant(cf.ByAutomationId("InCountry")).AsComboBox();
            if (country != null)
            {
                country.Patterns.ExpandCollapse.Pattern.Expand();
                country.Select("Romania");
            }
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InPhone")).AsTextBox().Enter("8977941223");
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InEmail")).AsTextBox().Enter("mksds@gmail.com");
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InPass")).AsTextBox().Enter("12345");
            mainWindow.FindFirstDescendant(cf.ByAutomationId("InCard")).AsTextBox().Enter("3216549873216547");
            mainWindow.FindFirstDescendant(cf.ByAutomationId("VipCheck")).AsCheckBox().Click();
            mainWindow.FindFirstDescendant(cf.ByName("Ok")).AsButton().Click();
            if (mainWindow.FindFirstDescendant(cf.ByName("Congratulations")) != null)
            {
                var congratulationsWindow = mainWindow.FindFirstDescendant(cf.ByName("Congratulations")).AsWindow();
                congratulationsWindow.FindFirstDescendant(cf.ByName("OK")).AsButton().Click();
            }
            else
            {
                var existingUserWindow = mainWindow.FindFirstDescendant(cf.ByName("Error")).AsWindow();
                existingUserWindow.FindFirstDescendant(cf.ByName("OK")).AsButton().Click();
                mainWindow.FindFirstDescendant(cf.ByName("Cancel")).AsButton().Click();
            }

            mainWindow.FindFirstDescendant(cf.ByName("Exit")).AsButton().Click();
            var exitWindow = mainWindow.FindFirstDescendant(cf.ByName("Exit")).AsWindow();
            exitWindow.FindFirstDescendant(cf.ByName("Yes")).AsButton().Click();
        }

        [TestMethod]
        public void TestFindMethods()
        {
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankSystem.exe");
            var application = FlaUI.Core.Application.Launch(exePath);

            //var application = FlaUI.Core.Application.Launch(@"D:\GitClonedRepo\FlaUIPractice-master\FlaUIPractice-master\FlaUIPractice\BankSystem\bin\Release\BankSystem.exe");
            var automation = new UIA3Automation();
            var mainWindow = application.GetMainWindow(automation);
            ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

            var elements = mainWindow.FindAll(FlaUI.Core.Definitions.TreeScope.Children,
                new PropertyCondition(automation.PropertyLibrary.Element.ControlType, FlaUI.Core.Definitions.ControlType.Edit));
            foreach (var item in elements)
            {
                item.DrawHighlight();
                Thread.Sleep(500);
            }
            //mainWindow.FindFirstDescendant(cf.ByControlType(FlaUI.Core.Definitions.ControlType.Edit)).DrawHighlight();
            //Thread.Sleep(500);
        }

        [TestMethod]
        public void TestMenuControls()
        {
            var application = FlaUI.Core.Application.Launch(@"D:\GitClonedRepo\FlaUIPractice-master\FlaUIPractice-master\FlaUIPractice\FlaUiTests\Resources\WinFormsApplication.exe");
            var automation = new UIA3Automation();
            var mainWindow = application.GetMainWindow(automation);
            ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());
            //var menu = mainWindow.FindFirstDescendant(cf.Menu()).AsMenu();
            //menu.DrawHighlight();

            //menu.Items["File"].Invoke();
            mainWindow.FindFirstDescendant(cf.ByName("ContextMenu")).AsButton().RightClick();
            var contextMenu = mainWindow.ContextMenu;
            contextMenu.DrawHighlight();
            contextMenu.Items[0].DrawHighlight();
        }

        [TestMethod]
        public void TestMouseActions()
        {
            // Ensure you are using System.Drawing.Point
            System.Drawing.Point point = new System.Drawing.Point(2298, 82);

            Mouse.MoveTo(1500, 100);
            Mouse.MoveBy(100, 100);
            Mouse.LeftClick();

            // Instead of Mouse.Click(MouseButton.Left, point);
            // Use this simpler version:
            Mouse.LeftClick(point);
        }

        [TestMethod]
        public void TestCaptureMethod()
        {
            string screenshotDir = @"D:\GitClonedRepo\FlaUIPractice-master\FlaUIPractice-master\FlaUIPractice\Screenshots";

            // This line solves the DirectoryNotFoundException
            Directory.CreateDirectory(screenshotDir);

            // 1. Full screen
            var fullscreenImg = Capture.Screen();
            fullscreenImg.ToFile(Path.Combine(screenshotDir, "Full Screen.png"));

            // 2. Only one automation element
            string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankSystem.exe");
            var application = FlaUI.Core.Application.Launch(exePath);

            //var application = FlaUI.Core.Application.Launch(@"D:\GitClonedRepo\FlaUIPractice-master\FlaUIPractice-master\FlaUIPractice\BankSystem\bin\Release\BankSystem.exe");
            var automation = new UIA3Automation();
            var mainWindow = application.GetMainWindow(automation);
            ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());

            var loginBtn = mainWindow.FindFirstDescendant(cf.ByName("Log In"));
            if (loginBtn != null)
            {
                var loginImg = Capture.Element(loginBtn);
                loginImg.ToFile(Path.Combine(screenshotDir, "Login Button.png"));
            }

            // 3. User defined rectangle area 
            var rectangleImg = Capture.Rectangle(new System.Drawing.Rectangle(500, 500, 100, 150));
            rectangleImg.ToFile(Path.Combine(screenshotDir, "Rectangle Img.png"));

            application.Close();
        }

        [TestMethod]
        public void TestPatterns()
        {

        }
    }
}