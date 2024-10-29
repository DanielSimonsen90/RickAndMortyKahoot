/**
 * Constant storage keys for the session storage.
 */
export const SESSION_STORAGE_KEYS = {
  USER: 'user',
  IS_HOST: 'isHost',
} as const;

/**
 * How long to wait until automatically ending a round.
 */
export const ANSWER_TIMEOUT_SECONDS = 60;

/**
 * How long to wait until automatically moving onto the next round after round is already ended.
 */
export const ROUND_END_TIMEOUT_SECONDS = 5;

/**
 * When to show critical loading blink
 */
export const CRITICAL_TIMEOUT_SECONDS = 10;