import type {
	MediaTypeItemResponseModel,
	MediaTypeResponseModel,
	MediaTypeTreeItemResponseModel,
} from '@umbraco-cms/backoffice/external/backend-api';

export type UmbMockMediaTypeModel = MediaTypeResponseModel &
	MediaTypeTreeItemResponseModel &
	MediaTypeItemResponseModel;

export type UmbMockMediaTypeUnionModel =
	| MediaTypeResponseModel
	| MediaTypeTreeItemResponseModel
	| MediaTypeItemResponseModel;

export const data: Array<UmbMockMediaTypeModel> = [
	{
		name: 'Image',
		id: 'media-type-1-id',
		parent: null,
		description: 'Media type 1 description',
		alias: 'mediaType1',
		icon: 'icon-picture',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'mediaPicker',
				name: 'Media Picker',
				description: '',
				dataType: { id: 'dt-uploadField' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
			{
				id: '5b4ca208-134e-4865-b423-06e5e97adf3c',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'mediaType1Property1',
				name: 'Media Type 1 Property 1',
				description: null,
				dataType: { id: '0cc0eba1-9960-42c9-bf9b-60e150b429ae' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 1,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
			{
				id: '7',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'listView',
				name: 'List View',
				description: '',
				dataType: { id: 'dt-collectionView' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 2,
				validation: {
					mandatory: false,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'media-type-1-id' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: false,
		aliasCanBeChanged: false,
	},
	{
		name: 'Audio',
		id: 'media-type-2-id',
		parent: null,
		description: 'Media type 2 description',
		alias: 'mediaType2',
		icon: 'icon-audio-lines',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'umbracoFile',
				name: 'File',
				description: '',
				dataType: { id: 'dt-uploadFieldFiles' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'media-type-2-id' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: false,
		aliasCanBeChanged: false,
	},
	{
		name: 'Vector Graphics',
		id: 'media-type-3-id',
		parent: null,
		description: 'Media type 3 description',
		alias: 'mediaType3',
		icon: 'icon-origami',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'umbracoFile',
				name: 'File',
				description: '',
				dataType: { id: 'dt-uploadFieldVector' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'media-type-3-id' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: false,
		aliasCanBeChanged: false,
	},
	{
		name: 'Movie',
		id: 'media-type-4-id',
		parent: null,
		description: 'Media type 4 description',
		alias: 'mediaType4',
		icon: 'icon-video',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'umbracoFile',
				name: 'File',
				description: '',
				dataType: { id: 'dt-uploadFieldMovies' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'media-type-4-id' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: false,
		aliasCanBeChanged: false,
	},
	{
		name: 'Media Type 5',
		id: 'media-type-5-id',
		parent: null,
		description: 'Media type 5 description',
		alias: 'mediaType5',
		icon: 'icon-document',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'umbracoFile',
				name: 'File',
				description: '',
				dataType: { id: 'dt-uploadFieldFiles' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'media-type-5-id' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: false,
		aliasCanBeChanged: false,
	},
	{
		name: 'A Forbidden Media Type',
		id: 'forbidden',
		parent: null,
		description: 'Clicking on this results in a 403 Forbidden error',
		alias: 'forbidden',
		icon: 'icon-document',
		properties: [
			{
				id: '19',
				container: { id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75' },
				alias: 'umbracoFile',
				name: 'File',
				description: '',
				dataType: { id: 'dt-uploadFieldFiles' },
				variesByCulture: false,
				variesBySegment: false,
				sortOrder: 0,
				validation: {
					mandatory: true,
					mandatoryMessage: null,
					regEx: null,
					regExMessage: null,
				},
				appearance: {
					labelOnTop: false,
				},
			},
		],
		containers: [
			{
				id: 'c3cd2f12-b7c4-4206-8d8b-27c061589f75',
				parent: null,
				name: 'Content',
				type: 'Group',
				sortOrder: 0,
			},
		],
		allowedAsRoot: true,
		variesByCulture: false,
		variesBySegment: false,
		isElement: false,
		allowedMediaTypes: [{ mediaType: { id: 'forbidden' }, sortOrder: 0 }],
		compositions: [],
		isFolder: false,
		hasChildren: false,
		collection: { id: 'dt-collectionView' },
		isDeletable: true,
		aliasCanBeChanged: false,
	},
];
