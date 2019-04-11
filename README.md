# nop-plugin-file-copier
Copies updated content file from nopCommerce plugin source directory to plugin output directory on save

Steps: 

1) Setup your config.json

[
  {
      "From": "C:\\MyProjects\\NopProject\\Plugins\\Nop.Plugin.Misc.MyPlugin",
      "To": "C:\\MyProjects\\NopProject\\Plugins\\Presentation\\Nop.Web\\Plugins\\Misc.MyPlugin"

  },
  {
      "From": "C:\\MyProjects\\NopProject\\Plugins\\Nop.Plugin.Payments.MyPaymentPlugin",
      "To": "C:\\MyProjects\\NopProject\\Plugins\\Presentation\\Nop.Web\\Plugins\\Payments.MyPaymentPlugin"

  }
]

2) Compile to executable and run any time you might modify your plugin cshtml / xml / js / css
