import type { NextApiRequest, NextApiResponse } from 'next';
import { prisma } from '../../../lib/prisma';
import { randomBytes } from 'crypto';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
  if (req.method !== 'POST') return res.status(405).end();
  const { userEmail, maxActivations } = req.body;
  const licenseKey = randomBytes(12).toString('hex');
  const user = await prisma.user.upsert({
    where: { email: userEmail },
    create: { email: userEmail },
    update: {}
  });
  const license = await prisma.license.create({
    data: {
      key: licenseKey,
      expiresAt: new Date(Date.now() + 1000 * 60 * 60 * 24 * 365),
      maxActivations: maxActivations ?? 1,
      userId: user.id
    }
  });
  res.json({ license });
}
