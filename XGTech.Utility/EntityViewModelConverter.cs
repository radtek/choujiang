using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Utility
{
    public class EntityViewModelConverter
    {
        public static TProduct ConvertSingle<TRaw, TProduct>(TRaw raw) where TProduct : new()
        {
            return ConvertSingle<TRaw, TProduct>(raw, new TProduct());
        }

        public static TProduct ConvertSingle<TRaw, TProduct>(TRaw raw, TProduct product) where TProduct : new()
        {
            Type t = product.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo productProperty in properties)
            {
                ConvertProperty(raw, product, productProperty);
            }
            return product;
        }

        private static void ConvertProperty<TRaw, TProduct>(TRaw raw, TProduct product, PropertyInfo productProperty) where TProduct : new()
        {
            foreach (PropertyInfo rawProperty in raw.GetType().GetProperties())
            {
                ConvertProperty(raw, product, productProperty, rawProperty);
            }
        }

        private static void ConvertProperty<TRaw, TProduct>(TRaw raw, TProduct product, PropertyInfo productProperty, PropertyInfo rawProperty) where TProduct : new()
        {
            if (productProperty.Name == rawProperty.Name)
            {
                var productAttr = (ConvertAttribute)productProperty.GetCustomAttribute(typeof(ConvertAttribute));
                var rawAttr = (ConvertAttribute)rawProperty.GetCustomAttribute(typeof(ConvertAttribute));

                if (IsPropertyException(productAttr, rawAttr))
                {
                    return;
                }

                if (productProperty.PropertyType == rawProperty.PropertyType)
                {
                    productProperty.SetValue(product, rawProperty.GetValue(raw));
                }
            }
        }

        private static bool IsPropertyException(ConvertAttribute productAttr, ConvertAttribute rawAttr)
        {
            if (null != productAttr && productAttr.HasException)
            {
                return true;
            }
            if (null != rawAttr && rawAttr.HasException)
            {
                return true;
            }
            return false;
        }

        public static IList<TProduct> ConvertList<TRaw, TProduct>(IList<TRaw> raws) where TProduct : new()
        {
            List<TProduct> products = new List<TProduct>();
            foreach (var raw in raws)
            {
                TProduct product = ConvertSingle<TRaw, TProduct>(raw);
                products.Add(product);
            }
            return products;
        }


        /// <summary>
        /// 将ViewModel转换为对应Entity类型，只转换名称相同的属性
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TEntity ViewModelToEntity<TViewModel, TEntity>(TViewModel model) where TEntity : new()
        {
            TEntity entity = new TEntity();
            Type t = entity.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo np in properties)
            {
                foreach (PropertyInfo p in model.GetType().GetProperties())
                {
                    if (np.Name == p.Name && np.PropertyType == p.PropertyType)
                    {
                        np.SetValue(entity, p.GetValue(model));
                    }
                }
            }
            return entity;
        }


        public static TViewModel EntityToViewModel<TEntity, TViewModel>(TEntity entity) where TViewModel : new()
        {
            TViewModel model = new TViewModel();
            Type t = model.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo np in properties)
            {
                foreach (PropertyInfo p in entity.GetType().GetProperties())
                {
                    if (np.Name == p.Name && np.PropertyType == p.PropertyType)
                    {
                        np.SetValue(model, p.GetValue(entity));
                    }
                }
            }
            return model;
        }

        public static IList<TEntity> ViewModelsToEntities<TViewModel, TEntity>(IList<TViewModel> models) where TEntity : new()
        {
            List<TEntity> entities = new List<TEntity>();
            foreach (var model in models)
            {
                TEntity entity = ViewModelToEntity<TViewModel, TEntity>(model);
                entities.Add(entity);
            }
            return entities;
        }

        public static IList<TViewModel> EntitiesToViewModels<TEntity, TViewModel>(IList<TEntity> entities) where TViewModel : new()
        {
            List<TViewModel> models = new List<TViewModel>();
            foreach (var entity in entities)
            {
                TViewModel model = EntityToViewModel<TEntity, TViewModel>(entity);
                models.Add(model);
            }
            return models;
        }
    }
}
