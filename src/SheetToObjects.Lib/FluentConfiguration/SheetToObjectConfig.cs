using System;

namespace SheetToObjects.Lib.FluentConfiguration
{
    public abstract class SheetToObjectConfig<TModel> where TModel : new()
    {
        public MappingConfig MappingConfig { get; protected set; }

        protected void CreateMap(Func<MappingConfigBuilder<TModel>, MappingConfigBuilder<TModel>> mappingConfigFunc)
        {
            MappingConfig = mappingConfigFunc(new MappingConfigBuilder<TModel>()).Build();
        }
    }
}