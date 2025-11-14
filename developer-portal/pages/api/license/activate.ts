import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../lib/prisma';
import jwt from 'jsonwebtoken';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'POST') return res.status(405).end();
  const { licenseKey, hardwareId } = req.body;
  const license = await prisma.license.findUnique({ where: { key: licenseKey } });
  if (!license) return res.status(404).json({ error: 'License not found' });
  const activations = await prisma.activation.count({ where: { licenseId: license.id } });
  if (activations >= license.maxActivations) return res.status(400).json({ error: 'Max activations reached' });
  await prisma.activation.create({ data: { licenseId: license.id, hardwareId } });
  const token = jwt.sign({ sub: license.id, uid: license.userId, maxActivations: license.maxActivations }, process.env.JWT_PRIVATE_KEY!, { algorithm: 'RS256', expiresIn: '30d' });
  res.json({ token });
}
