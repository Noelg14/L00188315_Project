# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

output "app_name" {
  value = azurerm_linux_web_app.ob_dash.name
}

output "app_url" {
  value = "https://${azurerm_linux_web_app.ob_dash.default_hostname}"
}