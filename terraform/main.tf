# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

provider "azurerm" {
  features {}
  subscription_id = "${var.subscription_id}"
  skip_provider_registration = true
}

resource "azurerm_service_plan" "atu-sp" {
  name                = "${var.prefix}-sp"
  location            = var.location
  resource_group_name = "ATU_CSD_2025"
  os_type             = "Windows"
  sku_name            = "F1"

}


resource "azurerm_windows_web_app" "app-service" {
  name                = "${var.prefix}-dashboard"
  location            = var.location
  resource_group_name = "ATU_CSD_2025"
  service_plan_id     = azurerm_service_plan.atu-sp.id


  site_config {
    always_on           = false
    application_stack {
      dotnet_version = "v8.0"
    }
  }
}