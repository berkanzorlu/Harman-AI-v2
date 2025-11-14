import './globals.css';
import type { ReactNode } from 'react';

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <body className="bg-slate-950 text-white">
        <div className="max-w-6xl mx-auto py-8">{children}</div>
      </body>
    </html>
  );
}
