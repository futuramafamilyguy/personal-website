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
