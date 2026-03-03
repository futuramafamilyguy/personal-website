terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.92"
    }
  }

  backend "s3" {
    bucket = "allenmaygibson-tf-state"
    key    = "personal_website/terraform.tfstate"
    region = "ap-southeast-2"
  }

  required_version = ">= 1.12.0, < 2.0.0"
}

provider "aws" {
  region = "ap-southeast-2"
}
