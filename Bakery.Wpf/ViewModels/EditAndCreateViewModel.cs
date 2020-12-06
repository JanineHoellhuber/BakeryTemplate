﻿using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Bakery.Persistence;
using Bakery.Wpf.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Bakery.Wpf.ViewModels
{
    internal class EditAndCreateViewModel : BaseViewModel
    {
        private IWindowController controller;
        private ProductDto selectedProduct;
        private Product _product;
        private string _productNr;
        private string _productName;
        private string _price;
        private bool create = false;

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }
        public string ProductNr
        {
            get => _productNr;
            set
            {
                _productNr = value;
                OnPropertyChanged(nameof(ProductNr));
            }
        }

      
        public string Name
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Price
        {
            get => _price;
            set
            {
                _price = value;
                OnErrorsChanged(nameof(Price));
            }
        }


        public EditAndCreateViewModel(IWindowController controller, ProductDto selectedProduct) : base(controller)
        {
            this.controller = controller;

            if (selectedProduct != null)
            {
                NewProduct();
            }
            else
            {
                create = true;
            }
        }

        public void NewProduct() {
            Product = new Product()
            {
                Id = selectedProduct.Id,
                ProductNr = selectedProduct.ProductNr,
                Name = selectedProduct.Name,
                Price = selectedProduct.Price,
            };
            ProductNr = selectedProduct.ProductNr;
            Name = selectedProduct.Name;
            Price = selectedProduct.Price.ToString();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new System.NotImplementedException();
        }
        private ICommand _cmdSaveCommand;

        public ICommand CmdSaveCommand
        {
            get
            {
                if (_cmdSaveCommand == null)
                {
                    _cmdSaveCommand = new RelayCommand(
                        execute: async _ =>
                        {
                            ValidateViewModelProperties();

                            try
                            {
                                await using IUnitOfWork uow = new UnitOfWork();

                                if (!create)
                                {
                                    Product productDb = await uow.Products.GetByIdAsync(Product.Id);
                                    productDb.ProductNr = ProductNr;
                                    productDb.Name = Name;
                                    productDb.Price = Convert.ToDouble(Price);
                                    uow.Products.Update(productDb);
                                }
                                else
                                {
                                    Product = new Product()
                                    {
                                        ProductNr = ProductNr,
                                        Name = Name,
                                        Price = Double.Parse(Price),
                                    };

                                    await uow.Products.AddAsync(Product);
                                }
                                await uow.SaveChangesAsync();
                                Controller.CloseWindow(this);
                            }
                            catch (ValidationException ex)
                            {
                                if (ex.Value is IEnumerable<string> properties)
                                {
                                    foreach (var property in properties)
                                    {
                                        Errors.Add(property, new List<string> { ex.ValidationResult.ErrorMessage });
                                    }
                                }
                                else
                                {
                                    DbError = ex.ValidationResult.ToString();
                                }
                            }

                        },
                        canExecute: _ => !HasErrors
                        );
                }

                return _cmdSaveCommand;
            }
            set => _cmdSaveCommand = value;
        }

        private ICommand _cmdCancelCommand;

        public ICommand CmdCancelCommand
        {
            get
            {
                if (_cmdCancelCommand == null)
                {
                    _cmdCancelCommand = new RelayCommand(
                        execute: _ =>
                        {
                            Controller.CloseWindow(this);
                        },
                        canExecute => true
                        );
                }

                return _cmdCancelCommand;
            }
            set => _cmdCancelCommand = value;
        }

    }
}