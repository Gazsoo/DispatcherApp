import type{ ReactNode } from 'react';

interface CardProps {
  children: ReactNode;
  className?: string;
}

export const Card = ({ children, className = '' }: CardProps) => {
  return (
    <div className={`bg-surface-light-card dark:bg-surface-dark-card backdrop-blur-sm rounded-2xl p-8 shadow-xl border border-surface-light-border dark:border-surface-dark-border max-w-md w-full ${className}`}>
      {children}
    </div>
  );
};
