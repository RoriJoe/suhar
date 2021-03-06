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
    public partial class sellingReportFPRangkap3 : Form
    {
        public String ID_Trans;
        public sellingReportFPRangkap3(String Trans_ID)
        {
            InitializeComponent();
            ID_Trans = Trans_ID;
            FPRangkap3_Multi multi = new FPRangkap3_Multi();
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

            multi.Subreports[0].SetDataSource(transprod);
            multi.Subreports[1].SetDataSource(transprod);
            multi.Subreports[2].SetDataSource(transprod);

            for(int i = 0;i < 3;i++)
            {
                multi.SetParameterValue("identityCompany", identityNow.Identity_Company_Name, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("identityCity", identityNow.Identity_City, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("identityName", identityNow.Identity_Name, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("identityAddress", identityNow.Identity_Address, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("TransactionDate", transNow.Customer_Transaction_Date.ToString("D", System.Globalization.CultureInfo.CreateSpecificCulture("id-ID")), multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("CustomerCompany", custNow.Customer_Company_Name, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("CustomerAddress", custNow.Customer_Address, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("CustomerNpwp", custNow.Customer_NPWP, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("identityNPWP", identityNow.Identity_NPWP, multi.Subreports[i].Name.ToString());
                multi.SetParameterValue("link page", i+1, multi.Subreports[i].Name.ToString());
            }

            _rpvFPR3.ReportSource = multi;
        }

        private void _rpvFPR3_Load(object sender, EventArgs e)
        {

        }

        private void sellingReportFPRangkap3_Load(object sender, EventArgs e)
        {

        }
    }
}
