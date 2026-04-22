using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication7
{
    public partial class Form1 : Form
    {
        // قاموس لتخزين أسعار الصرف، حيث يكون المفتاح هو رمز العملة والقيمة هي سعر الصرف.
        Dictionary<string, double> exchangeRates = new Dictionary<string, double>();

        // مسار ملف أسعار الصرف.
        string filePath = "exchangeRates";

        // دالة البناء للنموذج (Form1).
        public Form1()
        {
            // تهيئة مكونات النموذج.
            InitializeComponent();

            // تحميل أسعار الصرف عند بدء تشغيل البرنامج.
            LoadExchangeRates();
        }

        // دالة لتحميل أسعار الصرف من الملف.
        private void LoadExchangeRates()
        {
            // إذا كان الملف غير موجود، قم بإنشائه بالأسعار الافتراضية.
            if (!File.Exists(filePath))
            {
                // كتابة الأسعار الافتراضية في الملف.
                File.WriteAllText(filePath, "USD,3.75\nEUR,4.10\nGBP,4.85");

                // عرض رسالة للمستخدم لتحديث الأسعار.
                MessageBox.Show("تم انشاء اسعار الملف ,قم بتحديثه بالبيانات الصحيحه ,تنبيه", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // مسح القاموس الحالي لأسعار الصرف.
            exchangeRates.Clear();

            // قراءة جميع سطور الملف.
            string[] lines = File.ReadAllLines(filePath);

            // المرور على كل سطر في الملف.
            foreach (string line in lines)
            {
                // متغير لتخزين سعر الصرف.
                double rate;

                // تقسيم السطر إلى جزأين (رمز العملة وسعر الصرف).
                string[] parts = line.Split(',');

                // إذا كان السطر يحتوي على جزأين وكان سعر الصرف قابلًا للتحويل إلى عدد عشري.
                if (parts.Length == 2 && double.TryParse(parts[1], out rate))
                {
                    // إضافة رمز العملة وسعر الصرف إلى القاموس.
                    exchangeRates[parts[0].Trim()] = rate;
                }
            }

            // مسح عناصر صندوقي الاختيار.
            comboBoxForm.Items.Clear();
            comboBoxTo.Items.Clear();

            // إضافة رموز العملات إلى صندوقي الاختيار.
            foreach (var currency in exchangeRates.Keys)
            {
                comboBoxForm.Items.Add(currency);
                comboBoxTo.Items.Add(currency);
            }

            // تحديد العنصر الأول في صندوق الاختيار الأول.
            if (comboBoxForm.Items.Count > 0)
                comboBoxForm.SelectedIndex = 0;

            // تحديد العنصر الثاني في صندوق الاختيار الثاني.
            if (comboBoxTo.Items.Count > 1)
                comboBoxTo.SelectedIndex = 1;
        }

        // دالة لمعالجة حدث النقر على زر التحويل.
        private void buttonConvert_Click(object sender, EventArgs e)
        {
            // متغير لتخزين المبلغ المراد تحويله.
            double amount;

            // إذا كان المبلغ المدخل قابلًا للتحويل إلى عدد عشري.
            if (double.TryParse(textBoxAmount.Text, out amount))
            {
                // الحصول على رمز العملة من صندوق الاختيار الأول.
                string fromCurrency = comboBoxForm.SelectedItem.ToString();

                // الحصول على رمز العملة من صندوق الاختيار الثاني.
                string toCurrency = comboBoxTo.SelectedItem.ToString();

                // إذا كانت العملتان موجودتين في القاموس.
                if (exchangeRates.ContainsKey(fromCurrency) && exchangeRates.ContainsKey(toCurrency))
                {
                    // حساب المبلغ المحول.
                    double convertedAmount = amount * (exchangeRates[toCurrency] / exchangeRates[fromCurrency]);

                    // عرض المبلغ المحول في التسمية.
                    label5.Text = convertedAmount.ToString();
                }
                else
                {
                    // عرض رسالة خطأ إذا كانت العملات غير صحيحة.
                    MessageBox.Show("يرجى اختيار عملات صحيحه ,", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // عرض رسالة خطأ إذا كان المبلغ غير صحيح.
                MessageBox.Show("يرجى اختيار مبلغ صحيح ,", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // دالة لمعالجة حدث النقر على الزر 1 (يمكنك إضافة أي كود هنا).
        private void button1_Click(object sender, EventArgs e)
        {
            // يمكنك إضافة أي كود هنا إذا كنت بحاجة إلى تنفيذ شيء ما عند النقر على button1
        }
    }
}