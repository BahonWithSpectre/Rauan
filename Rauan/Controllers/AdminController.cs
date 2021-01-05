using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Rauan.DbData;
using Rauan.ViewModels;

namespace Rauan.Controllers
{
    public class AdminController : Controller
    {


        private AppDbContext db;
        private IHostingEnvironment env;

        public AdminController(AppDbContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        //List Actions
        public IActionResult List()
        {
            return View();
        }

        public IActionResult CatList()
        {
            return View(db.Categories.ToList());
        }

        public IActionResult PodCatList(int Id)
        {
            ViewBag.Cat = db.Categories.FirstOrDefault(p => p.Id == Id).Name;

            return View(db.Pod_Categories.Where(p => p.CategoryId == Id).ToList());
        }









        public async Task<IActionResult> ListProduct()
        {
            ShopViewModel svm = new ShopViewModel
            {
                Products = await db.Products.Include(p => p.Pod_Category).ThenInclude(p => p.Category).ToListAsync(),
                Categories = await db.Categories.ToListAsync(),
                
            };
            return View(svm);
        }

        public ActionResult ListRequest(string reqlist)
        {
            if (reqlist == "ListRequest")
            {
                IQueryable<Request> req = db.Requests;
                return PartialView(req);
            }
            return PartialView("error");
        }

        public ActionResult ListCategory(string ctlist)
        {
            if (ctlist == "ListCategory")
            {
                IQueryable<Category> ct = db.Categories;
                return PartialView(ct);
            }
            return PartialView("error");
        }
        public ActionResult ListPodCategory(string ctname)
        {
            int cid = db.Categories.FirstOrDefault(c => c.Name == ctname).Id;
            IQueryable<Pod_Category> pct = db.Pod_Categories.Where(p => p.CategoryId == cid);

            ViewBag.pctname = db.Categories.FirstOrDefault(c => c.Name == ctname).Name;

            return PartialView(pct);
        }
        public ActionResult PodCategory(string ctmodel)
        {
            int ct = db.Categories.FirstOrDefault(p => p.Name == ctmodel).Id;
            IQueryable<Pod_Category> pc = db.Pod_Categories.Where(p => p.CategoryId == ct);

            return PartialView(pc);
        }












        //Add Actions
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    Name = model.Name
                };
                await db.Categories.AddAsync(category);
                await db.SaveChangesAsync();
                return RedirectToAction("List", "Admin");
            }
            return View(model);
        }











        public IActionResult AddPodCategory()
        {
            ViewBag.Category = db.Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPodCategory(Pod_Category model, string ctmodel)
        {
            if (ModelState.IsValid)
            {
                int cid = db.Categories.FirstOrDefault(p => p.Name == ctmodel).Id;

                Pod_Category podcat = new Pod_Category
                {
                    Name = model.Name,
                    CategoryId = cid
                };
                await db.Pod_Categories.AddAsync(podcat);
                await db.SaveChangesAsync();
                return RedirectToAction("PodCatList", "Admin", new { Id = cid });
            }
            return View(model);
        }





        public IActionResult AddProduct()
        {
            ViewBag.Category = db.Categories.AsNoTracking().ToList();
            ViewBag.pc = db.Pod_Categories.AsNoTracking().ToList();
            ViewBag.Brand = db.Brands.AsNoTracking().ToList();

            return View(new Product());
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductModel model, IFormFile PublicImg, string ctmodel, string pctmodel, int brand)
        {
            if (ModelState.IsValid)
            {
                if (PublicImg != null)
                {
                    string pimg = "/PublicImages/" + PublicImg.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(env.WebRootPath + pimg, FileMode.Create))
                    {
                        await PublicImg.CopyToAsync(fileStream);
                    }
                }
                //int cid = db.Categories.FirstOrDefault(p => p.Name == ctmodel).Id;
                int pcid = db.Pod_Categories.FirstOrDefault(p => p.Name == pctmodel).Id;

                Product product = new Product
                {
                    Name = model.Name,
                    FirstInfo = model.FirstInfo,
                    Price = model.Price,
                    Info1 = model.Info1,
                    Info2 = model.Info2,
                    Info3 = model.Info3,
                    Info4 = model.Info4,
                    Info5 = model.Info5,
                    PublicImage = PublicImg.FileName,
                    Pod_CategoryId = pcid,
                    BrandId = brand
                };

                var ewres = await db.BrandPodCategories.Where(p => p.Pod_CategoryId == pcid && p.BrandId == brand).FirstOrDefaultAsync();
                if(ewres == null)
                {
                    BrandPodCategory bpc = new BrandPodCategory { BrandId = pcid, Pod_CategoryId = brand };
                    await db.BrandPodCategories.AddAsync(bpc);
                }

                await db.Products.AddAsync(product);
                await db.SaveChangesAsync();
                return RedirectToAction("EditProduct", "Admin", new { product.Id });
            }
            return View(model);
        }

        public async Task<IActionResult> AddOnInfo(int? Id, string InName)
        {
            if (Id != null)
            {
                Product pr = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);
                if (pr != null)
                {
                    if (InName != null)
                    {
                        ProductInfo productInfo = new ProductInfo { InfoName = InName, ProductId = pr.Id };

                        await db.ProductInfos.AddAsync(productInfo);
                        await db.SaveChangesAsync();
                        return RedirectToAction("EditProduct", "Admin", new { Id });
                    }
                    return RedirectToAction("EditProduct", "Admin");
                }

            }
            return RedirectToAction("List", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> AddInInfo(int? Id, string InName, string InVal)
        {
            if (Id != null)
            {
                ProductInfo pr = await db.ProductInfos.FirstOrDefaultAsync(p => p.Id == Id);
                if (pr != null)
                {
                    Id = pr.ProductId;
                    if ((InName != null) && (InVal != null))
                    {
                        InProductInfo productInfo = new InProductInfo { InfoName = InName, InfoValue = InVal, ProductInfoId = pr.Id };

                        await db.InProductInfos.AddAsync(productInfo);
                        await db.SaveChangesAsync();

                        return RedirectToAction("EditProduct", "Admin", new { Id });
                    }
                    return RedirectToAction("EditProduct", "Admin", new { Id });
                }

            }
            return RedirectToAction("List", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> AddPhoto(int? Id, IFormFile addphoto)
        {
            Product pr = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if ((addphoto != null) && (pr != null))
            {
                string path = "/ProductImages/" + addphoto.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(env.WebRootPath + path, FileMode.Create))
                {
                    await addphoto.CopyToAsync(fileStream);
                }

                ProductImage pm = new ProductImage { ProductId = pr.Id, Url = addphoto.FileName };
                await db.ProductImages.AddAsync(pm);
                await db.SaveChangesAsync();

                return RedirectToAction("EditProduct", "Admin", new { pr.Id });
            }

            return RedirectToAction("EditProduct", "Admin", new { Id });
        }








        //Edit Actions
        public async Task<IActionResult> EditProductMain(int? Id)
        {
            if (Id != null)
            {
                ViewBag.Category = db.Categories.AsNoTracking().ToList();
                ViewBag.pc = db.Pod_Categories.AsNoTracking().ToList();
                ViewBag.Brand = db.Brands.AsNoTracking().ToList();

                Product pr = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);

                return View(pr);
            }
            return RedirectToAction("ListProduct", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> EditProductMain(int? Id, ProductModel model, IFormFile PublicImg, string ctmodel, string pctmodel, int brand)
        {
            if (Id != null)
            {
                Product pr = await db.Products.FirstOrDefaultAsync(p => p.Id == Id);
                if (PublicImg != null)
                {
                    string pimg = "/PublicImages/" + PublicImg.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(env.WebRootPath + pimg, FileMode.Create))
                    {
                        await PublicImg.CopyToAsync(fileStream);
                    }
                    pr.PublicImage = PublicImg.FileName;
                }
                int pcid = db.Pod_Categories.FirstOrDefault(p => p.Name == pctmodel).Id;

                if (pr != null)
                {
                    pr.Name = model.Name;
                    pr.FirstInfo = model.FirstInfo;
                    pr.Price = model.Price;
                    pr.Info1 = model.Info1;
                    pr.Info2 = model.Info2;
                    pr.Info3 = model.Info3;
                    pr.Info4 = model.Info4;
                    pr.Info5 = model.Info5;
                    pr.Pod_CategoryId = pcid;
                    pr.BrandId = brand;
                    await db.SaveChangesAsync();
                    return RedirectToAction("EditProduct", "Admin", new { pr.Id });
                }

            }
            return RedirectToAction("ListProduct", "Admin");
        }
        public async Task<IActionResult> EditProduct(int? Id)
        {
            if (Id != null)
            {
                Product product = await db.Products.Include(p => p.Pod_Category).ThenInclude(p => p.Category).FirstOrDefaultAsync(p => p.Id == Id);
                IEnumerable<Comment> cm = await db.Comments.Where(p => p.ProductId == product.Id).ToListAsync();
                IEnumerable<ProductImage> pi = await db.ProductImages.Where(p => p.ProductId == product.Id).ToListAsync();
                IEnumerable<ProductInfo> pinfo = await db.ProductInfos.Where(p => p.ProductId == product.Id).Include(p => p.InProductInfos).ToListAsync();

                if (product != null)
                {
                    DetailsViewModel dm = new DetailsViewModel
                    {
                        Products = product,
                        Comments = cm,
                        ProductImages = pi,
                        ProductInfos = pinfo
                    };
                    return View(dm);
                }
            }
            return RedirectToAction("ListProduct", "Admin");
        }

        public IActionResult EditAllPhotos(int Id)
        {
            var list = db.ProductImages.Where(p => p.ProductId == Id).ToList();

            ViewBag.Name = db.Products.Find(Id).Name;
            ViewBag.Id = db.Products.Find(Id).Id; ;

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> EditAllPhotos(IFormFile img, ProductImage model)
        {
            var pi = db.ProductImages.FirstOrDefault(p => p.Id == model.Id);

            if(img != null)
            {
                string pimg = "/ProductImages/" + img.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(env.WebRootPath + pimg, FileMode.Create))
                {
                    await img.CopyToAsync(fileStream);
                }

                pi.Url = img.FileName;

                db.ProductImages.Update(pi);
                await db.SaveChangesAsync();
            }

            return RedirectToAction("EditAllPhotos", new { Id = pi.ProductId });
        }


        public IActionResult DeleteAllPhotos(int Id)
        {
            var pi = db.ProductImages.FirstOrDefault(p => p.Id == Id);

            db.ProductImages.Remove(pi);

            db.SaveChanges();

            return RedirectToAction("EditAllPhotos", new { Id = pi.ProductId });
        }





        public IActionResult EditProductInfo(int Id)
        {
            var dd = db.ProductInfos.FirstOrDefault(p => p.Id == Id);

            return View(dd);

        }

        [HttpPost]
        public IActionResult EditProductInfo(ProductInfo model)
        {
            db.ProductInfos.Update(model);
            db.SaveChanges();

            return RedirectToAction("EditProduct", new { Id = model.ProductId });

        }


        public IActionResult EditInProductInfo(int Id)
        {
            var dd = db.InProductInfos.Include(p=>p.ProductInfo).FirstOrDefault(p => p.Id == Id);

            return View(dd);

        }

        [HttpPost]
        public IActionResult EditInProductInfo(InProductInfo model)
        {
            var date = db.InProductInfos.Include(p=>p.ProductInfo).FirstOrDefault(p=>p.Id == model.Id);

            date.InfoName = model.InfoName;
            date.InfoValue = model.InfoValue;

            db.InProductInfos.Update(date);
            db.SaveChanges();

            return RedirectToAction("EditProduct", new { Id = date.ProductInfo.ProductId });

        }







        public IActionResult EditCategory(int? Id)
        {
            var dd = db.Categories.FirstOrDefault(x => x.Id == Id);
            return View(dd);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            var ct = db.Categories.FirstOrDefault(p => p.Id == model.Id);

            if (ct != null)
            {
                ct.Name = model.Name;
                db.Categories.Update(ct);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("CatList", "Admin");
        }











        public IActionResult EditPodCategory(int? Id)
        {
            var pcid = db.Pod_Categories.Include(p=>p.Category).FirstOrDefault(p => p.Id == Id);

            return View(pcid);
        }


        [HttpPost]
        public async Task<IActionResult> EditPodCategory(Pod_Category model)
        {
            var pct = db.Pod_Categories.FirstOrDefault(p => p.Id == model.Id);

            if (pct != null)
            {
                pct.Name = model.Name;
                db.Pod_Categories.Update(pct);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("PodCatList", "Admin", new { Id = pct.CategoryId });
        }
















        //Delete Actions
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id != null)
            {
                Category ct = db.Categories.FirstOrDefault(c => c.Id == id);

                db.Categories.Remove(ct);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("CatList", "Admin");
        }
        public async Task<IActionResult> DeletePodCategory(int? id)
        {
            if (id != null)
            {
                Pod_Category pct = db.Pod_Categories.FirstOrDefault(c => c.Id == id);

                db.Pod_Categories.Remove(pct);
                await db.SaveChangesAsync();

                return RedirectToAction("PodCatList", "Admin", new { Id =pct.CategoryId  });
            }
            return RedirectToAction("CatList", "Admin");
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id != null)
            {
                Product product = db.Products.FirstOrDefault(p => p.Id == id);

                db.Products.Remove(product);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("ListProduct", "Admin");
        }




        public IActionResult Banners()
        {

            var list = db.Banners.ToList();

            //List<Banner> bl = new List<Banner>();

            //bl.Add(new Banner { Number = 1, Url = "" });
            //bl.Add(new Banner { Number = 1, Url = "" });
            //bl.Add(new Banner { Number = 1, Url = "" });

            //bl.Add(new Banner { Number = 2, Url = "" });
            //bl.Add(new Banner { Number = 2, Url = "" });
            //bl.Add(new Banner { Number = 2, Url = "" });

            //bl.Add(new Banner { Number = 3, Url = "" });
            //bl.Add(new Banner { Number = 3, Url = "" });
            //bl.Add(new Banner { Number = 3, Url = "" });

            //db.Banners.AddRange(bl);


            //db.SaveChanges();

            return View(list);
        }

        public IActionResult EditBanner(int Id)
        {
            var ban = db.Banners.FirstOrDefault(o => o.Id == Id);

            return View(ban);
        }


        [HttpPost]
        public async Task<IActionResult> EditBanner(Banner model, IFormFile banner)
        {

            if (banner != null)
            {
                string pimg = "/banner/" + banner.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(env.WebRootPath + pimg, FileMode.Create))
                {
                    await banner.CopyToAsync(fileStream);
                }

                var ban = db.Banners.FirstOrDefault(o => o.Id == model.Id);
                if(ban != null)
                {
                    ban.Url = pimg;

                    db.Banners.Update(ban);
                    db.SaveChanges();
                }
            }



            return RedirectToAction("Banners");
        }



        public IActionResult Brand()
        {
            return View(db.Brands.ToList());
        }

        public IActionResult AddBrand()
        {
            return View(new Brand());
        }

        [HttpPost]
        public IActionResult AddBrand(Brand model)
        {
            db.Brands.Add(model);
            db.SaveChanges();

            return RedirectToAction("Brand", "Admin");
        }


        public IActionResult EditBrand(int Id)
        {
            return View(db.Brands.Where(p=>p.Id == Id).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult EditBrand(Brand model)
        {
            var br = db.Brands.Where(p => p.Id == model.Id).FirstOrDefault();

            br.BrandName = model.BrandName;
            br.Icon = model.Icon;

            db.Brands.Update(br);
            db.SaveChanges();

            return RedirectToAction("Brand", "Admin");
        }
    }
}
