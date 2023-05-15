using HRM201c;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.Json;

IWebDriver driver = new ChromeDriver();
IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

string email = "Chitnhs171469@fpt.edu.vn";
string password = "ngocchitran9";

driver.Navigate().GoToUrl("https://www.coursera.org/?authMode=login");

driver.FindElement(By.Name("email")).SendKeys(email);
driver.FindElement(By.Name("password")).SendKeys(password);
driver.FindElement(By.CssSelector(@"button[data-e2e='login-form-submit-button']")).Click();

Console.WriteLine("đéo có tiền thuê api thì nhập capcha đi...");
WaitPassCaptcha();

//Mooc 1
driver.Navigate().GoToUrl("https://www.coursera.org/learn/managing-human-resources/exam/DosiJ/different-approaches-to-managing-people/attempt");
Thread.Sleep(10000);

var lengthOfQuestion = (long)js.ExecuteScript("return document.querySelectorAll('.rc-FormPartsQuestion').length");

string jsonString = File.ReadAllText(@"C:\Users\NTH\Desktop\ToolCoursera\HRM201c\HRM201c\Mooc1_1.json");
var listAnswer = JsonSerializer.Deserialize<List<AnswerModel>>(jsonString);

for (int i = 1; i <= lengthOfQuestion; i++)
{
    var questionContent = (string)js.ExecuteScript("return document.querySelector('.rc-FormPartsQuestion:nth-child(" + i + ")>.rc-FormPartsQuestion__row>.rc-FormPartsQuestion__contentCell').textContent");
    foreach (var item in listAnswer)
    {
        //So khớp câu hỏi trên browser với câu hỏi file Mooc1_1.json
        if (questionContent.Trim() == item.Question.Trim())
        {
            var lengthOfAnswer = (long)js.ExecuteScript("return document.querySelectorAll('.rc-FormPartsQuestion:nth-child("+i+")>.rc-FormPartsQuestion__row:nth-child(2)>.rc-FormPartsQuestion__contentCell>div>.css-1mkdutb').length");

            if (lengthOfAnswer != 0)
            {
                for (int j = 1; j <= lengthOfAnswer; j++)
                {
                    var answer = (string)js.ExecuteScript("return document.querySelector('.rc-FormPartsQuestion:nth-child("+i+")>.rc-FormPartsQuestion__row:nth-child(2)>.rc-FormPartsQuestion__contentCell>div>.css-1mkdutb:nth-child("+j+")>.rc-Option>label').textContent");
                    if (string.Join(",", item.Answers).Trim().Contains(answer.Trim()))
                    {
                        //Trường hợp 1 câu trả lời
                        if (item.Type == 0)
                        {
                            js.ExecuteScript("document.querySelector('.rc-FormPartsQuestion:nth-child(" + i + ")>.rc-FormPartsQuestion__row:nth-child(2)>.rc-FormPartsQuestion__contentCell>div>.css-1mkdutb:nth-child(" + j + ")>.rc-Option>label').click()");
                            break;
                        }
                        //Trường hợp nhiều câu trả lời
                        else if (item.Type == 1)
                        {
                            js.ExecuteScript("document.querySelector('.rc-FormPartsQuestion:nth-child(" + i + ")>.rc-FormPartsQuestion__row:nth-child(2)>.rc-FormPartsQuestion__contentCell>div>.css-1mkdutb:nth-child(" + j + ")>.rc-Option>label').click()");
                        }
                    }
                }
            }
            else
            {
                //Trường hợp câu trả lời là điền từ
                js.ExecuteScript("document.querySelector('.rc-FormPartsQuestion:nth-child(" + i + ")>.rc-FormPartsQuestion__row:nth-child(2)>.rc-FormPartsQuestion__contentCell>.rc-TextInputBox>._1lrtjdg>._kxlijz>._6xfqva').value = '" + item.Answers[0] + "'");
            }
            break;
        }
    }
}

js.ExecuteScript("document.querySelector('#agreement-checkbox-base').click()");
js.ExecuteScript("document.getElementsByClassName('cds-134 cds-105 cds-107  css-1yqs4tx cds-116 cds-button-disableElevation')[1].click()");


//Đợi để thằng lol vượt chaptcha login xong
void WaitPassCaptcha()
{
    string url = driver.Url;
    while (url == "https://www.coursera.org/?authMode=login")
    {
        Thread.Sleep(1000);
        url = driver.Url;
    }
}

