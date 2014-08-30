﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
using StockModel;


namespace StockApps
{
    public partial class sellingReportFP_bForm_RptViewer : Form
    {
        public String ID_Trans;
        public sellingReportFP_bForm_RptViewer(String Trans_ID)
        {
            InitializeComponent();
            ID_Trans = Trans_ID;
            sellingReportFP_cViewer rptFP = new sellingReportFP_cViewer();
            identity identityNow = IdentityController.getIdentity();

            var listTrans = CustomerTransaction.getCustomerTransaction();
            customer_transaction transNow = CustomerTransaction.getCustomerTransaction(ID_Trans).First();
            var custNow = transNow.customer;

            var listProduct = ProductController.getProduct();
            var listTransProd = transNow.customer_transaction_product;

            var transprod = listTransProd
                .Join(listProduct,
                customer_transaction_product => customer_transaction_product.Product_ID,
                product => product.Product_ID,
                (customer_transaction_product, product) => new { customer_transaction_product = customer_transaction_product, product = product })
                .Join(listTrans,
                join => join.customer_transaction_product.Customer_Transaction_ID,
                customer_transaction => customer_transaction.Customer_Transaction_ID,
                (join, customer_transaction) => new { join = join, customer_transaction = customer_transaction })
                .AsEnumerable()
                .Select(join => new
                {
                    Product_ID = join.join.customer_transaction_product.Product_ID + "",
                    Product_Name = join.join.product.Product_Name + "",
                    Product_Packing_Name = join.join.product.Product_Packing_Name + "",
                    Product_Packing_Kilogram = join.join.product.Product_Packing_Kilogram + "",
                    Customer_Transaction_ID = join.join.customer_transaction_product.Customer_Transaction_ID + "",
                    Customer_Transaction_Product_Quantity = join.join.customer_transaction_product.Customer_Transaction_Product_Quantity + "",
                    Customer_Transaction_Product_Price_Dollar = join.join.customer_transaction_product.Customer_Transaction_Product_Price_Dollar + "",
                    Customer_Transaction_Product_Total_Dollar = join.join.customer_transaction_product.Customer_Transaction_Product_Total_Dollar + "",
                    Customer_Transaction_Product_Price_Rupiah = join.join.customer_transaction_product.Customer_Transaction_Product_Price_Rupiah + "",
                    Customer_Transaction_Product_Total_Rupiah = join.join.customer_transaction_product.Customer_Transaction_Product_Total_Rupiah + "",
                    Currency_ID = join.customer_transaction.Currency_ID + "",
                    Customer_Transaction_Kurs = join.customer_transaction.Customer_Transaction_Kurs + "",
                    Customer_Transaction_Dollar = join.customer_transaction.Customer_Transaction_Dollar + "",
                    Customer_Transaction_Rupiah = join.customer_transaction.Customer_Transaction_Rupiah + "",
                    Customer_Transaction_PPN_Dollar = join.customer_transaction.Customer_Transaction_PPN_Dollar + "",
                    Customer_Transaction_PPN_Rupiah = join.customer_transaction.Customer_Transaction_PPN_Rupiah + "",
                    Customer_Transaction_Total_Dollar = join.customer_transaction.Customer_Transaction_Total_Dollar + "",
                    Customer_Transaction_Total_Rupiah = join.customer_transaction.Customer_Transaction_Total_Rupiah + ""
                }).ToList();

            rptFP.SetDataSource(transprod);
            rptFP.SetParameterValue("identityCompany", identityNow.Identity_Company_Name);
            rptFP.SetParameterValue("identityCity", identityNow.Identity_City);
            rptFP.SetParameterValue("IdentityName", identityNow.Identity_Name);
            rptFP.SetParameterValue("TransactionDate", identityNow.Identity_City + ", " + transNow.Customer_Transaction_Date.ToString("D", System.Globalization.CultureInfo.CreateSpecificCulture("id-ID")));
            rptFP.SetParameterValue("TransactionNoteNumber", transNow.Customer_Transaction_Note_Number);
            rptFP.SetParameterValue("CustomerCompany", custNow.Customer_Company_Name+"\n" +custNow.Customer_Address + "\n" + custNow.Customer_City);
            rptFP.SetParameterValue("terbilang", "Terbilang : " + (transNow.Currency_ID == 1 ? CurrencyController.decryptMoney(transNow.Customer_Transaction_Total_Dollar,1) : CurrencyController.decryptMoney(transNow.Customer_Transaction_Total_Rupiah,2)));
            _rptSRFakturPenjualan.ReportSource = rptFP;
        }

        private void _rptSRFakturPenjualan_Load(object sender, EventArgs e)
        {

        }

  
    }
}
