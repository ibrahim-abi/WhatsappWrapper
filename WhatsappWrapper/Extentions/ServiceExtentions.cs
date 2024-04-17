namespace WhatsappWrapper.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigurationCors(this IServiceCollection services)
        {
            services.AddCors(options => { options.AddDefaultPolicy(builder => builder.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()); });

        }
        public static void ConfigureIISIntegration(this IServiceCollection services) { services.Configure<IISOptions>(options => { }); }
    }
}
