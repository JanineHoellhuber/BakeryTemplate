using Bakery.Core.Contracts;
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
        private ProductDto _selectedProduct;
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

        [MinLength(1, ErrorMessage = "Produktname muss mindestens 1 Zeichen lang sein")]
        [MaxLength(20, ErrorMessage = "Produktname darf maximal 20 Zeichen lang sein")]
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
            _selectedProduct = selectedProduct;
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
                Id = _selectedProduct.Id,
                ProductNr = _selectedProduct.ProductNr,
                Name = _selectedProduct.Name,
                Price = _selectedProduct.Price,
            };
            ProductNr = _selectedProduct.ProductNr;
            Name = _selectedProduct.Name;
            Price = _selectedProduct.Price.ToString();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult> 
            { 
                ValidationResult.Success
            };
        }
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null)
                {
                    _cmdSave = new RelayCommand(
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
                                    productDb.Price = Double.Parse(Price);
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

                return _cmdSave;
            }
            set => _cmdSave = value;
        }

        private ICommand _cmdUndo;

        public ICommand CmdUndo
        {
            get
            {
                if (_cmdUndo == null)
                {
                    _cmdUndo = new RelayCommand(
                        execute: _ =>
                        {
                            if (create)
                            {
                                ProductNr = string.Empty;
                                Name = string.Empty;
                                Price = string.Empty;
                                return;
                            }

                            ProductNr = _product.ProductNr;
                            Name = _product.Name;
                            Price = _product.Price.ToString();
                        },
                        canExecute => true
                        );
                }

                return _cmdUndo;
            }
            set => _cmdUndo = value;
        }

     
    }
}