import type { ButtonHTMLAttributes, ReactNode } from 'react';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
  children: ReactNode;
  isLoading?: boolean;
}

export const Button = ({
  variant = 'primary',
  className = '',
  isLoading = false,
  children,
  ...props
}: ButtonProps) => {
  const baseStyles = "w-full font-semibold py-3 px-6 rounded-lg transition-all duration-300";

  const variants = {
    primary: "bg-gradient-to-r from-accent to-accent-dark hover:from-accent-dark hover:to-accent text-white shadow-lg hover:shadow-xl",
    secondary: "bg-surface-light-border dark:bg-surface-dark-border hover:bg-accent/20 dark:hover:bg-accent/20 text-content-light dark:text-content-dark font-medium border-2 border-transparent hover:border-accent duration-200"
  };

  return (
    <button
      className={`${baseStyles} ${variants[variant]} ${isLoading ? 'opacity-70 cursor-not-allowed' : ''} ${className}`}
      disabled={isLoading || props.disabled}
      {...props}
    >
      {isLoading ? (
        <span className="flex items-center justify-center gap-2">
          <svg className="animate-spin h-5 w-5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          Loading...
        </span>
      ) : (
        children
      )}
    </button>
  );
};
