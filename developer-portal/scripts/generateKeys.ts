import { generateKeyPairSync } from 'crypto';

const { privateKey, publicKey } = generateKeyPairSync('rsa', {
  modulusLength: 2048,
});

console.log('Private Key:\n', privateKey.export({ type: 'pkcs1', format: 'pem' }).toString());
console.log('Public Key:\n', publicKey.export({ type: 'pkcs1', format: 'pem' }).toString());
