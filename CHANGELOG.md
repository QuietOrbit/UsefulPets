# Changelog

All notable changes to this project will be documented in this file.

This project adheres to [Semantic Versioning](https://semver.org/).

---

## [0.1.0] - 2025-05-15

### Added
- Harmony patch for `HoeDirt.destroyCrop` to block crow crop destruction when protection is active
- Random daily protection chance based on pet friendship (linear scaling)
- Logging of crop protection decisions and friendship levels
- `CrowService` to manage protection logic and track saved crops
- `PetService` to access pet friendship and player’s pet data
- `SafeHelper` utility to wrap potentially risky logic and ensure game stability
- `LogHelper` to provide consistent, context-rich mod logging
