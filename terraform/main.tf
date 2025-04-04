# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg-atu" {
  name     = "${var.prefix}-resources"
  location = var.location
}

resource "azurerm_service_plan" "atu-sp" {
  name                = "${var.prefix}-sp"
  location            = azurerm_resource_group.rg-atu.location
  resource_group_name = azurerm_resource_group.rg-atu.name
  os_type             = "Linux"
  sku_name            = "F1"
}


resource "azurerm_linux_web_app" "ob_dash" {
  name                = "${var.prefix}-dashboard"
  location            = azurerm_resource_group.rg-atu.location
  resource_group_name = azurerm_resource_group.rg-atu.name
  service_plan_id     = azurerm_service_plan.atu-sp.id

  site_config {
    application_stack {
      dotnet_version =  8.0.x
    }
  }
}