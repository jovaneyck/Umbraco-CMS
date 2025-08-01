import type { UfmPlugin } from './types.js';
import type { MarkedExtension } from '@umbraco-cms/backoffice/external/marked';

/**
 * @param {Array<UfmPlugin>} plugins - An array of UFM plugins.
 * @returns {MarkedExtension} A Marked extension object.
 */
export function ufm(plugins: Array<UfmPlugin> = []): MarkedExtension {
	return {
		extensions: plugins.map(({ alias, marker, render }) => {
			const prefix = `(${alias}:${marker ? `|${marker}` : ''})`;
			return {
				name: alias,
				level: 'inline',
				start: (src: string) => src.search(`{${prefix}`),
				tokenizer: (src: string) => {
					const pattern = `^\\{${prefix}([^}]*)\\}`;
					const regex = new RegExp(pattern);
					const match = src.match(regex);

					if (match) {
						const [raw, prefix, content = ''] = match;
						return {
							type: alias,
							raw: raw,
							tokens: [],
							prefix: prefix,
							text: content.trim(),
						};
					}

					return undefined;
				},
				renderer: render,
			};
		}),
	};
}
