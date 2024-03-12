# TransformNewMailProvisionerData

## Overview
TransformNewMailProvisionerData is a utility tool designed for system administrators and IT professionals. It facilitates the migration of user accounts from a legacy system to a new platform by pulling all previously provisioned accounts, matching them against Active Directory (AD) to determine active accounts, and then re-importing them in a normalized fashion into another system. The tool also includes functionality to check for active licenses associated with each account, ensuring a smooth transition and continuity of service for users.

## Features
- **Data Extraction**: Extract account data from legacy systems efficiently.
- **AD Integration**: Match extracted accounts against Active Directory to filter active accounts.
- **Data Normalization**: Normalize account data to fit the requirements of the new system.
- **License Verification**: Check and verify the license status of each account to ensure active users have the necessary access rights.
- **Log Generation**: Generate detailed logs of the migration process for auditing and troubleshooting purposes.

## Getting Started

### Prerequisites
- .NET Core 6 or higher
- Access to the legacy system's database
- Access to Active Directory
- Permissions to read from and write to the target system

### Installation
1. Clone the repository to your local machine:
   ```bash
   git clone [https://github.com/yourusername/TransformNewMailProvisionerData.git](https://github.com/briandarley/Transform-Mail-Provisioner.git)https://github.com/briandarley/Transform-Mail-Provisioner.git
