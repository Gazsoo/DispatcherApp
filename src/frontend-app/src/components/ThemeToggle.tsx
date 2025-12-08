import { useState, useEffect } from 'react'

function ThemeToggle({ className }: { className?: string }) {
  const [darkMode, setDarkMode] = useState(() => {
    // Check localStorage first
    const saved = localStorage.getItem('theme')
    if (saved) return saved === 'dark'

    // Otherwise check system preference
    return window.matchMedia('(prefers-color-scheme: dark)').matches
  })

  useEffect(() => {
    const root = document.documentElement

    if (darkMode) {
      root.classList.add('dark')
      root.style.colorScheme = 'dark'
      localStorage.setItem('theme', 'dark')
    } else {
      root.classList.remove('dark')
      root.style.colorScheme = 'light'
      localStorage.setItem('theme', 'light')
    }
  }, [darkMode])

  return (
    <button
      onClick={() => setDarkMode(!darkMode)}
      className={`relative w-16 h-8 bg-surface-light-border dark:bg-surface-dark-border rounded-full transition-colors duration-300 shadow-lg ${className}`}
      aria-label="Toggle dark mode"
    >
      <span
        className={`absolute top-1 left-1 w-6 h-6 bg-white dark:bg-surface-dark-card rounded-full shadow-md transform transition-transform duration-300 flex items-center justify-center text-sm ${darkMode ? 'translate-x-8' : 'translate-x-0'
          }`}
      >
        {darkMode ? 'D' : 'L'}
      </span>
    </button>
  )
}

export default ThemeToggle