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

variable "domain_name" {
  type    = string
  default = "allenmaygibson.com"
}

resource "aws_lightsail_container_service" "personal_website_client_service" {
  name  = "personal-website-client-service"
  power = "small"
  scale = 1
}

resource "aws_lightsail_container_service_deployment_version" "personal_website_client_deployment" {
  container {
    container_name = "client"
    image          = "ghcr.io/futuramafamilyguy/personal-website-client:latest"

    ports = {
      3000 = "HTTP"
    }
  }

  public_endpoint {
    container_name = "client"
    container_port = 3000

    health_check {
      unhealthy_threshold = 3
      timeout_seconds     = 5
      interval_seconds    = 30
    }
  }

  service_name = aws_lightsail_container_service.personal_website_client_service.name
}

resource "aws_secretsmanager_secret" "personal_website" {
  name = "personal-website"
}

data "aws_secretsmanager_secret_version" "personal_website_value" {
  secret_id = aws_secretsmanager_secret.personal_website.id
}

locals {
  secrets = jsondecode(data.aws_secretsmanager_secret_version.personal_website_value.secret_string)
}

resource "aws_lightsail_container_service" "personal_website_server_service" {
  name  = "personal-website-server-service"
  power = "small"
  scale = 1
}

resource "aws_lightsail_container_service_deployment_version" "personal_website_server_deployment" {
  container {
    container_name = "server"
    image          = "ghcr.io/futuramafamilyguy/personal-website-server:latest"

    environment = {
      ImageStorageConfiguration__Provider      = "S3"
      ImageStorageConfiguration__Host          = "https://s3.ap-southeast-2.amazonaws.com/"
      ImageStorageConfiguration__CdnEnabled    = "true"
      MarkdownStorageConfiguration__Provider   = "S3"
      MarkdownStorageConfiguration__Host       = "https://s3.ap-southeast-2.amazonaws.com/"
      MarkdownStorageConfiguration__CdnEnabled = "true"
      AllowedOrigins                           = "https://${var.domain_name},https://www.${var.domain_name}"
      CdnConfiguration__Host                   = "https://cdn.${var.domain_name}"
      ConnectionStrings__PersonalWebsiteDb     = local.secrets["ConnectionStrings__PersonalWebsiteDb"]
      AWS_ACCESS_KEY_ID                        = local.secrets["AWS_ACCESS_KEY_ID"]
      AWS_SECRET_ACCESS_KEY                    = local.secrets["AWS_SECRET_ACCESS_KEY"]
      AWS_REGION                               = "ap-southeast-2"
      S3Configuration__BucketName              = local.secrets["S3Configuration__BucketName"]
      BasicAuthConfiguration__AdminUsername    = local.secrets["BasicAuthConfiguration__AdminUsername"]
      BasicAuthConfiguration__AdminPassword    = local.secrets["BasicAuthConfiguration__AdminPassword"]
    }

    ports = {
      8080 = "HTTP"
    }
  }

  public_endpoint {
    container_name = "server"
    container_port = 8080

    health_check {
      unhealthy_threshold = 3
      timeout_seconds     = 5
      interval_seconds    = 30
      path                = "/health"
    }
  }

  service_name = aws_lightsail_container_service.personal_website_server_service.name
}
