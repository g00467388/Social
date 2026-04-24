import { Preferences } from '@capacitor/preferences';

export class ThemeService {
  private static instance: ThemeService;

  private themes = ['light', 'dark'];
  private key = 'theme';

  private constructor() { }

  static getInstance(): ThemeService {
    return ThemeService.instance == null
      ? ThemeService.instance = new ThemeService()
      : ThemeService.instance;
  }

  async loadTheme() {
    const { value } = await Preferences.get({ key: this.key });
    this.applyTheme(value || 'light');
  }

  async toggleTheme() {
    const { value } = await Preferences.get({ key: this.key });
    const current = value || 'light';

    const next = this.getNextTheme(current);

    this.applyTheme(next);
    await Preferences.set({ key: this.key, value: next });
  }

  private getNextTheme(current: string): string {
    return current === 'light'
      ? 'dark'
      : 'light';
  }

  private applyTheme(theme: string) {
    this.themes.forEach(t => document.documentElement.classList.remove(t));
    document.documentElement.classList.add(theme);
  }
}
