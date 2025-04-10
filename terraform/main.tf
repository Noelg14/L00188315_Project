# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

provider "azurerm" {
  features {}
  subscription_id = "${var.subscription_id}"
  skip_provider_registration = true
}

resource "azurerm_resource_group" "rg" {
  name     = "${var.prefix}-rg"
  location = var.location
}

resource "azurerm_service_plan" "sp" {
  name                = "${var.prefix}-sp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  os_type             = "Linux"
  sku_name            = "F1"

}


resource "azurerm_linux_web_app" "app-service" {
  name                = "${var.prefix}-dashboard"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  service_plan_id     = azurerm_service_plan.sp.id


  site_config {
    always_on           = false
    application_stack {
      dotnet_version =  "8.0"
    }
  }
}