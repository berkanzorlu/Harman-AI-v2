# License System

## Token
- RS256 JWT signed with private key stored on portal backend.
- Claims: `sub` (license id), `uid`, `maxActivations`, `expires`, `graceDays`, `hardwareHashes`.

## Activation Flow
1. Desktop collects hardware fingerprint and license key from user.
2. Sends to `/api/license/activate`.
3. Portal validates license, ensures activation count < `maxActivations`, records hardware hash.
4. Returns JWT token + activation id. Token cached locally encrypted via DPAPI.

## Heartbeat & Verification
- Desktop pings `/api/license/verify` every 6 hours with token and hardware hash.
- Portal verifies signature, expiration, and revocation flags.
- If offline, desktop accepts cached token for up to `graceDays`.

## Revocation
- Admin can revoke license or activation via portal.
- Revocation recorded in `licenses.revoked_at` and `activations.revoked_at`.

## Security
- Public key bundled with desktop for signature verification.
- Tamper detection checks binary hash and license.bin integrity using HMAC.
