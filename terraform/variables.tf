# Copyright (c) HashiCorp, Inc.
# SPDX-License-Identifier: MPL-2.0

variable "prefix" {
  type        = string
  description = "The prefix used for all resources in this example"
}

variable "location" {
  type        = string
  description = "The Azure location where all resources in this example should be created"
  default     = "europe-north"
}
variable "subscription_id" {
  type        = string
  description = "The Azure subscription where all resources in this example should be created"
}
variable "os" {
  type        = string
  description = "The Operating System the Resource will use - Windows or Linux"
  default = "Windows"
}