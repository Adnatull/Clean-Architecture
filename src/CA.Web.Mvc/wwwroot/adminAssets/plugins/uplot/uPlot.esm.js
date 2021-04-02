/**
* Copyright (c) 2020, Leon Sorokin
* All rights reserved. (MIT Licensed)
*
* uPlot.js (μPlot)
* A small, fast chart for time series, lines, areas, ohlc & bars
* https://github.com/leeoniya/uPlot (v1.4.4)
*/

function debounce(fn, time) {
	let pending = null;

	function run() {
		pending = null;
		fn();
	}

	return function() {
		clearTimeout(pending);
		pending = setTimeout(run, time);
	}
}

// binary search for index of closest value
function closestIdx(num, arr, lo, hi) {
	let mid;
	lo = lo || 0;
	hi = hi || arr.length - 1;
	let bitwise = hi <= 2147483647;

	while (hi - lo > 1) {
		mid = bitwise ? (lo + hi) >> 1 : floor((lo + hi) / 2);

		if (arr[mid] < num)
			lo = mid;
		else
			hi = mid;
	}

	if (num - arr[lo] <= arr[hi] - num)
		return lo;

	return hi;
}

function getMinMax(data, _i0, _i1, sorted) {
//	console.log("getMinMax()");

	let _min = inf;
	let _max = -inf;

	if (sorted == 1) {
		_min = data[_i0];
		_max = data[_i1];
	}
	else if (sorted == -1) {
		_min = data[_i1];
		_max = data[_i0];
	}
	else {
		for (let i = _i0; i <= _i1; i++) {
			if (data[i] != null) {
				_min = min(_min, data[i]);
				_max = max(_max, data[i]);
			}
		}
	}

	return [_min, _max];
}

const _fixedTuple = [0, 0];

function fixIncr(minIncr, maxIncr, minExp, maxExp) {
	_fixedTuple[0] = minExp < 0 ? roundDec(minIncr, -minExp) : minIncr;
	_fixedTuple[1] = maxExp < 0 ? roundDec(maxIncr, -maxExp) : maxIncr;
	return _fixedTuple;
}

function rangeLog(min, max, base, fullMags) {
	let logFn = base == 10 ? log10 : log2;

	if (min == max) {
		min /= base;
		max *= base;
	}

	let minExp, maxExp, minMaxIncrs;

	if (fullMags) {
		minExp = floor(logFn(min));
		maxExp =  ceil(logFn(max));

		minMaxIncrs = fixIncr(pow(base, minExp), pow(base, maxExp), minExp, maxExp);

		min = minMaxIncrs[0];
		max = minMaxIncrs[1];
	}
	else {
		minExp = floor(logFn(min));
		maxExp = floor(logFn(max));

		minMaxIncrs = fixIncr(pow(base, minExp), pow(base, maxExp), minExp, maxExp);

		min = incrRoundDn(min, minMaxIncrs[0]);
		max = incrRoundUp(max, minMaxIncrs[1]);
	}

	return [min, max];
}

const _eqRangePart = {
	pad:  0,
	soft: null,
	mode: 0,
};

const _eqRange = {
	min: _eqRangePart,
	max: _eqRangePart,
};

// this ensures that non-temporal/numeric y-axes get multiple-snapped padding added above/below
// TODO: also account for incrs when snapping to ensure top of axis gets a tick & value
function rangeNum(_min, _max, mult, extra) {
	if (isObj(mult))
		return _rangeNum(_min, _max, mult);

	_eqRangePart.pad  = mult;
	_eqRangePart.soft = extra ? 0 : null;
	_eqRangePart.mode = extra ? 2 : 0;

	return _rangeNum(_min, _max, _eqRange);
}

// nullish coalesce
function ifNull(lh, rh) {
	return lh == null ? rh : lh;
}

function _rangeNum(_min, _max, cfg) {
	let cmin = cfg.min;
	let cmax = cfg.max;

	let padMin = ifNull(cmin.pad, 0);
	let padMax = ifNull(cmax.pad, 0);

	let hardMin = ifNull(cmin.hard, -inf);
	let hardMax = ifNull(cmax.hard,  inf);

	let softMin = ifNull(cmin.soft,  inf);
	let softMax = ifNull(cmax.soft, -inf);

	let softMinMode = ifNull(cmin.mode, 0);
	let softMaxMode = ifNull(cmax.mode, 0);

	let delta        = _max - _min;
	let nonZeroDelta = delta || abs(_max) || 1e3;
	let mag          = log10(nonZeroDelta);
	let base         = pow(10, floor(mag));

	let _padMin  = nonZeroDelta * (delta == 0 ? (_min == 0 ? .1 : 1) : padMin);
	let _newMin  = roundDec(incrRoundDn(_min - _padMin, base/100), 6);
	let _softMin = _min >= softMin && (softMinMode == 1 || softMinMode == 2 && _newMin < softMin) ? softMin : inf;
	let minLim   = max(hardMin, _newMin < _softMin && _min >= _softMin ? _softMin : min(_softMin, _newMin));

	let _padMax  = nonZeroDelta * (delta == 0 ? (_max == 0 ? .1 : 1) : padMax);
	let _newMax  = roundDec(incrRoundUp(_max + _padMax, base/100), 6);
	let _softMax = _max <= softMax && (softMaxMode == 1 || softMaxMode == 2 && _newMax > softMax) ? softMax : -inf;
	let maxLim   = min(hardMax, _newMax > _softMax && _max <= _softMax ? _softMax : max(_softMax, _newMax));

	if (minLim == maxLim && minLim == 0)
		maxLim = 100;

	return [minLim, maxLim];
}

// alternative: https://stackoverflow.com/a/2254896
const fmtNum = new Intl.NumberFormat(navigator.language).format;

const M = Math;

const abs = M.abs;
const floor = M.floor;
const round = M.round;
const ceil = M.ceil;
const min = M.min;
const max = M.max;
const pow = M.pow;
const log10 = M.log10;
const log2 = M.log2;
const PI = M.PI;

const inf = Infinity;

function incrRound(num, incr) {
	return round(num/incr)*incr;
}

function clamp(num, _min, _max) {
	return min(max(num, _min), _max);
}

function fnOrSelf(v) {
	return typeof v == "function" ? v : () => v;
}

function retArg1(_0, _1) {
	return _1;
}

function incrRoundUp(num, incr) {
	return ceil(num/incr)*incr;
}

function incrRoundDn(num, incr) {
	return floor(num/incr)*incr;
}

function roundDec(val, dec) {
	return round(val * (dec = 10**dec)) / dec;
}

const fixedDec = new Map();

function guessDec(num) {
	return ((""+num).split(".")[1] || "").length;
}

function genIncrs(base, minExp, maxExp, mults) {
	let incrs = [];

	let multDec = mults.map(guessDec);

	for (let exp = minExp; exp < maxExp; exp++) {
		let expa = abs(exp);
		let mag = roundDec(pow(base, exp), expa);

		for (let i = 0; i < mults.length; i++) {
			let _incr = mults[i] * mag;
			let dec = (_incr >= 0 && exp >= 0 ? 0 : expa) + (exp >= multDec[i] ? 0 : multDec[i]);
			let incr = roundDec(_incr, dec);
			incrs.push(incr);
			fixedDec.set(incr, dec);
		}
	}

	return incrs;
}

//export const assign = Object.assign;

const EMPTY_OBJ = {};

const isArr = Array.isArray;

function isStr(v) {
	return typeof v === 'string';
}

function isObj(v) {
	return typeof v === 'object' && v !== null;
}

function copy(o) {
	let out;

	if (isArr(o))
		out = o.map(copy);
	else if (isObj(o)) {
		out = {};
		for (var k in o)
			out[k] = copy(o[k]);
	}
	else
		out = o;

	return out;
}

function assign(targ) {
	let args = arguments;

	for (let i = 1; i < args.length; i++) {
		let src = args[i];

		for (let key in src) {
			if (isObj(targ[key]))
				assign(targ[key], copy(src[key]));
			else
				targ[key] = copy(src[key]);
		}
	}

	return targ;
}

const microTask = typeof queueMicrotask == "undefined" ? fn => Promise.resolve().then(fn) : queueMicrotask;

const WIDTH = "width";
const HEIGHT = "height";
const TOP = "top";
const BOTTOM = "bottom";
const LEFT = "left";
const RIGHT = "right";
const firstChild = "firstChild";
const createElement = "createElement";
const hexBlack = "#000";
const transparent = hexBlack + "0";
const classList = "classList";

const mousemove = "mousemove";
const mousedown = "mousedown";
const mouseup = "mouseup";
const mouseenter = "mouseenter";
const mouseleave = "mouseleave";
const dblclick = "dblclick";
const resize = "resize";
const scroll = "scroll";

const pre = "u-";

const UPLOT          =       "uplot";
const TITLE          = pre + "title";
const WRAP           = pre + "wrap";
const UNDER          = pre + "under";
const OVER           = pre + "over";
const OFF            = pre + "off";
const SELECT         = pre + "select";
const CURSOR_X       = pre + "cursor-x";
const CURSOR_Y       = pre + "cursor-y";
const CURSOR_PT      = pre + "cursor-pt";
const LEGEND         = pre + "legend";
const LEGEND_LIVE    = pre + "live";
const LEGEND_INLINE  = pre + "inline";
const LEGEND_THEAD   = pre + "thead";
const LEGEND_SERIES  = pre + "series";
const LEGEND_MARKER  = pre + "marker";
const LEGEND_LABEL   = pre + "label";
const LEGEND_VALUE   = pre + "value";

const rAF = requestAnimationFrame;
const doc = document;
const win = window;
const pxRatio = devicePixelRatio;

function addClass(el, c) {
	c != null && el[classList].add(c);
}

function remClass(el, c) {
	el[classList].remove(c);
}

function setStylePx(el, name, value) {
	el.style[name] = value + "px";
}

function placeTag(tag, cls, targ, refEl) {
	let el = doc[createElement](tag);

	if (cls != null)
		addClass(el, cls);

	if (targ != null)
		targ.insertBefore(el, refEl);

	return el;
}

function placeDiv(cls, targ) {
	return placeTag("div", cls, targ);
}

function trans(el, xPos, yPos, xMax, yMax) {
	el.style.transform = "translate(" + xPos + "px," + yPos + "px)";

	if (xPos < 0 || yPos < 0 || xPos > xMax || yPos > yMax)
		addClass(el, OFF);
	else
		remClass(el, OFF);
}

const evOpts = {passive: true};

function on(ev, el, cb) {
	el.addEventListener(ev, cb, evOpts);
}

function off(ev, el, cb) {
	el.removeEventListener(ev, cb, evOpts);
}

const months = [
	"January",
	"February",
	"March",
	"April",
	"May",
	"June",
	"July",
	"August",
	"September",
	"October",
	"November",
	"December",
];

const days = [
	"Sunday",
	"Monday",
	"Tuesday",
	"Wednesday",
	"Thursday",
	"Friday",
	"Saturday",
];

function slice3(str) {
	return str.slice(0, 3);
}

const days3 =  days.map(slice3);

const months3 =  months.map(slice3);

const engNames = {
	MMMM: months,
	MMM:  months3,
	WWWW: days,
	WWW:  days3,
};

function zeroPad2(int) {
	return (int < 10 ? '0' : '') + int;
}

function zeroPad3(int) {
	return (int < 10 ? '00' : int < 100 ? '0' : '') + int;
}

/*
function suffix(int) {
	let mod10 = int % 10;

	return int + (
		mod10 == 1 && int != 11 ? "st" :
		mod10 == 2 && int != 12 ? "nd" :
		mod10 == 3 && int != 13 ? "rd" : "th"
	);
}
*/

const getFullYear = 'getFullYear';
const getMonth = 'getMonth';
const getDate = 'getDate';
const getDay = 'getDay';
const getHours = 'getHours';
const getMinutes = 'getMinutes';
const getSeconds = 'getSeconds';
const getMilliseconds = 'getMilliseconds';

const subs = {
	// 2019
	YYYY:	d => d[getFullYear](),
	// 19
	YY:		d => (d[getFullYear]()+'').slice(2),
	// July
	MMMM:	(d, names) => names.MMMM[d[getMonth]()],
	// Jul
	MMM:	(d, names) => names.MMM[d[getMonth]()],
	// 07
	MM:		d => zeroPad2(d[getMonth]()+1),
	// 7
	M:		d => d[getMonth]()+1,
	// 09
	DD:		d => zeroPad2(d[getDate]()),
	// 9
	D:		d => d[getDate](),
	// Monday
	WWWW:	(d, names) => names.WWWW[d[getDay]()],
	// Mon
	WWW:	(d, names) => names.WWW[d[getDay]()],
	// 03
	HH:		d => zeroPad2(d[getHours]()),
	// 3
	H:		d => d[getHours](),
	// 9 (12hr, unpadded)
	h:		d => {let h = d[getHours](); return h == 0 ? 12 : h > 12 ? h - 12 : h;},
	// AM
	AA:		d => d[getHours]() >= 12 ? 'PM' : 'AM',
	// am
	aa:		d => d[getHours]() >= 12 ? 'pm' : 'am',
	// a
	a:		d => d[getHours]() >= 12 ? 'p' : 'a',
	// 09
	mm:		d => zeroPad2(d[getMinutes]()),
	// 9
	m:		d => d[getMinutes](),
	// 09
	ss:		d => zeroPad2(d[getSeconds]()),
	// 9
	s:		d => d[getSeconds](),
	// 374
	fff:	d => zeroPad3(d[getMilliseconds]()),
};

function fmtDate(tpl, names) {
	names = names || engNames;
	let parts = [];

	let R = /\{([a-z]+)\}|[^{]+/gi, m;

	while (m = R.exec(tpl))
		parts.push(m[0][0] == '{' ? subs[m[1]] : m[0]);

	return d => {
		let out = '';

		for (let i = 0; i < parts.length; i++)
			out += typeof parts[i] == "string" ? parts[i] : parts[i](d, names);

		return out;
	}
}

const localTz = new Intl.DateTimeFormat().resolvedOptions().timeZone;

// https://stackoverflow.com/questions/15141762/how-to-initialize-a-javascript-date-to-a-particular-time-zone/53652131#53652131
function tzDate(date, tz) {
	let date2;

	// perf optimization
	if (tz == 'Etc/UTC')
		date2 = new Date(+date + date.getTimezoneOffset() * 6e4);
	else if (tz == localTz)
		date2 = date;
	else {
		date2 = new Date(date.toLocaleString('en-US', {timeZone: tz}));
		date2.setMilliseconds(date[getMilliseconds]());
	}

	return date2;
}

//export const series = [];

// default formatters:

const onlyWhole = v => v % 1 == 0;

const allMults = [1,2,2.5,5];

const wholeMults = allMults.filter(onlyWhole);

// ...0.01, 0.02, 0.025, 0.05, 0.1, 0.2, 0.25, 0.5
const decIncrs = genIncrs(10, -16, 0, allMults);

// 1, 2, 2.5, 5, 10, 20, 25, 50...
const oneIncrs = genIncrs(10, 0, 16, allMults);

// 1, 2,      5, 10, 20, 25, 50...
const wholeIncrs = oneIncrs.filter(onlyWhole);

const numIncrs = decIncrs.concat(oneIncrs);

let s = 1,
	m = 60,
	h = m * m,
	d = h * 24,
	mo = d * 30,
	y = d * 365;

// min of 1e-3 prevents setting a temporal x ticks too small since Date objects cannot advance ticks smaller than 1ms
const timeIncrs =  genIncrs(10, -3, 0, wholeMults).concat([
	// minute divisors (# of secs)
	1,
	5,
	10,
	15,
	30,
	// hour divisors (# of mins)
	m,
	m * 5,
	m * 10,
	m * 15,
	m * 30,
	// day divisors (# of hrs)
	h,
	h * 2,
	h * 3,
	h * 4,
	h * 6,
	h * 8,
	h * 12,
	// month divisors TODO: need more?
	d,
	d * 2,
	d * 3,
	d * 4,
	d * 5,
	d * 6,
	d * 7,
	d * 8,
	d * 9,
	d * 10,
	d * 15,
	// year divisors (# months, approx)
	mo,
	mo * 2,
	mo * 3,
	mo * 4,
	mo * 6,
	// century divisors
	y,
	y * 2,
	y * 5,
	y * 10,
	y * 25,
	y * 50,
	y * 100,
]);

// base 2
const binIncrs = genIncrs(2, -53, 53, [1]);

/*
console.log({
	decIncrs,
	oneIncrs,
	wholeIncrs,
	numIncrs,
	timeIncrs,
	fixedDec,
});
*/

function timeAxisStamps(stampCfg, fmtDate) {
	return stampCfg.map(s => s.map((v, i) =>
		i == 0 || i == 8 || v == null ? v : fmtDate(i == 1 || s[8] == 0 ? v : s[1] + v)
	));
}

const NL = "\n";

const yyyy = "{YYYY}";
const NLyyyy = NL + yyyy;
const md = "{M}/{D}";
const NLmd = NL + md;
const NLmdyy = NLmd + "/{YY}";

const aa = "{aa}";
const hmm = "{h}:{mm}";
const hmmaa = hmm + aa;
const NLhmmaa = NL + hmmaa;
const ss = ":{ss}";

const _ = null;

// [0]:   minimum num secs in the tick incr
// [1]:   default tick format
// [2-7]: rollover tick formats
// [8]:   mode: 0: replace [1] -> [2-7], 1: concat [1] + [2-7]
const _timeAxisStamps = [
//   tick incr    default          year                    month   day                   hour    min       sec   mode
	[y,           yyyy,            _,                      _,      _,                    _,      _,        _,       1],
	[d * 28,      "{MMM}",         NLyyyy,                 _,      _,                    _,      _,        _,       1],
	[d,           md,              NLyyyy,                 _,      _,                    _,      _,        _,       1],
	[h,           "{h}" + aa,      NLmdyy,                 _,      NLmd,                 _,      _,        _,       1],
	[m,           hmmaa,           NLmdyy,                 _,      NLmd,                 _,      _,        _,       1],
	[s,           ss,              NLmdyy + " " + hmmaa,   _,      NLmd + " " + hmmaa,   _,      NLhmmaa,  _,       1],
	[1e-3,        ss + ".{fff}",   NLmdyy + " " + hmmaa,   _,      NLmd + " " + hmmaa,   _,      NLhmmaa,  _,       1],
];

// TODO: will need to accept spaces[] and pull incr into the loop when grid will be non-uniform, eg for log scales.
// currently we ignore this for months since they're *nearly* uniform and the added complexity is not worth it
function timeAxisVals(tzDate, stamps) {
	return (self, splits, axisIdx, foundSpace, foundIncr) => {
		let s = stamps.find(s => foundIncr >= s[0]) || stamps[stamps.length - 1];

		// these track boundaries when a full label is needed again
		let prevYear;
		let prevMnth;
		let prevDate;
		let prevHour;
		let prevMins;
		let prevSecs;

		return splits.map(split => {
			let date = tzDate(split);

			let newYear = date[getFullYear]();
			let newMnth = date[getMonth]();
			let newDate = date[getDate]();
			let newHour = date[getHours]();
			let newMins = date[getMinutes]();
			let newSecs = date[getSeconds]();

			let stamp = (
				newYear != prevYear && s[2] ||
				newMnth != prevMnth && s[3] ||
				newDate != prevDate && s[4] ||
				newHour != prevHour && s[5] ||
				newMins != prevMins && s[6] ||
				newSecs != prevSecs && s[7] ||
				                       s[1]
			);

			prevYear = newYear;
			prevMnth = newMnth;
			prevDate = newDate;
			prevHour = newHour;
			prevMins = newMins;
			prevSecs = newSecs;

			return stamp(date);
		});
	}
}

// for when axis.values is defined as a static fmtDate template string
function timeAxisVal(tzDate, dateTpl) {
	let stamp = fmtDate(dateTpl);
	return (self, splits, axisIdx, foundSpace, foundIncr) => splits.map(split => stamp(tzDate(split)));
}

function mkDate(y, m, d) {
	return new Date(y, m, d);
}

// the ensures that axis ticks, values & grid are aligned to logical temporal breakpoints and not an arbitrary timestamp
// https://www.timeanddate.com/time/dst/
// https://www.timeanddate.com/time/dst/2019.html
// https://www.epochconverter.com/timezones
function timeAxisSplits(tzDate) {
	return (self, axisIdx, scaleMin, scaleMax, foundIncr, foundSpace) => {
		let splits = [];
		let isYr = foundIncr >= y;
		let isMo = foundIncr >= mo && foundIncr < y;

		// get the timezone-adjusted date
		let minDate = tzDate(scaleMin);
		let minDateTs = minDate / 1e3;

		// get ts of 12am (this lands us at or before the original scaleMin)
		let minMin = mkDate(minDate[getFullYear](), isYr ? 0 : minDate[getMonth](), isMo || isYr ? 1 : minDate[getDate]());
		let minMinTs = minMin / 1e3;

		if (isMo || isYr) {
			let moIncr = isMo ? foundIncr / mo : 0;
			let yrIncr = isYr ? foundIncr / y  : 0;
		//	let tzOffset = scaleMin - minDateTs;		// needed?
			let split = minDateTs == minMinTs ? minDateTs : mkDate(minMin[getFullYear]() + yrIncr, minMin[getMonth]() + moIncr, 1) / 1e3;
			let splitDate = new Date(split * 1e3);
			let baseYear = splitDate[getFullYear]();
			let baseMonth = splitDate[getMonth]();

			for (let i = 0; split <= scaleMax; i++) {
				let next = mkDate(baseYear + yrIncr * i, baseMonth + moIncr * i, 1);
				let offs = next - tzDate(next / 1e3);

				split = (+next + offs) / 1e3;

				if (split <= scaleMax)
					splits.push(split);
			}
		}
		else {
			let incr0 = foundIncr >= d ? d : foundIncr;
			let tzOffset = floor(scaleMin) - floor(minDateTs);
			let split = minMinTs + tzOffset + incrRoundUp(minDateTs - minMinTs, incr0);
			splits.push(split);

			let date0 = tzDate(split);

			let prevHour = date0[getHours]() + (date0[getMinutes]() / m) + (date0[getSeconds]() / h);
			let incrHours = foundIncr / h;

			let minSpace = self.axes[axisIdx]._space;
			let pctSpace = foundSpace / minSpace;

			while (1) {
				split = roundDec(split + foundIncr, 3);

				if (split > scaleMax)
					break;

				if (incrHours > 1) {
					let expectedHour = floor(roundDec(prevHour + incrHours, 6)) % 24;
					let splitDate = tzDate(split);
					let actualHour = splitDate.getHours();

					let dstShift = actualHour - expectedHour;

					if (dstShift > 1)
						dstShift = -1;

					split -= dstShift * h;

					prevHour = (prevHour + incrHours) % 24;

					// add a tick only if it's further than 70% of the min allowed label spacing
					let prevSplit = splits[splits.length - 1];
					let pctIncr = roundDec((split - prevSplit) / foundIncr, 3);

					if (pctIncr * pctSpace >= .7)
						splits.push(split);
				}
				else
					splits.push(split);
			}
		}

		return splits;
	}
}

function timeSeriesStamp(stampCfg, fmtDate) {
	return fmtDate(stampCfg);
}
const _timeSeriesStamp = '{YYYY}-{MM}-{DD} {h}:{mm}{aa}';

function timeSeriesVal(tzDate, stamp) {
	return (self, val) => stamp(tzDate(val));
}

function cursorPoint(self, si) {
	let s = self.series[si];

	let pt = placeDiv();

	pt.style.background = s.stroke || hexBlack;

	let dia = ptDia(s.width, 1);
	let mar = (dia - 1) / -2;

	setStylePx(pt, WIDTH, dia);
	setStylePx(pt, HEIGHT, dia);
	setStylePx(pt, "marginLeft", mar);
	setStylePx(pt, "marginTop", mar);

	return pt;
}

function dataIdx(self, seriesIdx, cursorIdx) {
	return cursorIdx;
}

const moveTuple = [0,0];

function cursorMove(self, mouseLeft1, mouseTop1) {
	moveTuple[0] = mouseLeft1;
	moveTuple[1] = mouseTop1;
	return moveTuple;
}

function filtBtn0(self, targ, handle) {
	return e => {
		e.button == 0 && handle(e);
	};
}

function passThru(self, targ, handle) {
	return handle;
}

const cursorOpts = {
	show: true,
	x: true,
	y: true,
	lock: false,
	move: cursorMove,
	points: {
		show: cursorPoint,
	},

	bind: {
		mousedown:   filtBtn0,
		mouseup:     filtBtn0,
		click:       filtBtn0,
		dblclick:    filtBtn0,

		mousemove:   passThru,
		mouseleave:  passThru,
		mouseenter:  passThru,
	},

	drag: {
		setScale: true,
		x: true,
		y: false,
		dist: 0,
		uni: null,
		_x: false,
		_y: false,
	},

	focus: {
		prox: -1,
	},

	left: -10,
	top: -10,
	idx: null,
	dataIdx,
};

const grid = {
	show: true,
	stroke: "rgba(0,0,0,0.07)",
	width: 2,
//	dash: [],
	filter: retArg1,
};

const ticks = assign({}, grid, {size: 10});

const font      = '12px system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial, "Noto Sans", sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji"';
const labelFont = "bold " + font;
const lineMult = 1.5;		// font-size multiplier

const xAxisOpts = {
	show: true,
	scale: "x",
	space: 50,
	gap: 5,
	size: 50,
	labelSize: 30,
	labelFont,
	side: 2,
//	class: "x-vals",
//	incrs: timeIncrs,
//	values: timeVals,
//	filter: retArg1,
	grid,
	ticks,
	font,
	rotate: 0,
};

const numSeriesLabel = "Value";
const timeSeriesLabel = "Time";

const xSeriesOpts = {
	show: true,
	scale: "x",
	auto: false,
	sorted: 1,
//	label: "Time",
//	value: v => stamp(new Date(v * 1e3)),

	// internal caches
	min: inf,
	max: -inf,
	idxs: [],
};

function numAxisVals(self, splits, axisIdx, foundSpace, foundIncr) {
	return splits.map(v => v == null ? "" : fmtNum(v));
}

function numAxisSplits(self, axisIdx, scaleMin, scaleMax, foundIncr, foundSpace, forceMin) {
	let splits = [];

	let numDec = fixedDec.get(foundIncr) || 0;

	scaleMin = forceMin ? scaleMin : roundDec(incrRoundUp(scaleMin, foundIncr), numDec);

	for (let val = scaleMin; val <= scaleMax; val = roundDec(val + foundIncr, numDec))
		splits.push(Object.is(val, -0) ? 0 : val);		// coalesces -0

	return splits;
}

function logAxisSplits(self, axisIdx, scaleMin, scaleMax, foundIncr, foundSpace, forceMin) {
	const splits = [];

	const logBase = self.scales[self.axes[axisIdx].scale].log;

	const logFn = logBase == 10 ? log10 : log2;

	const exp = floor(logFn(scaleMin));

	foundIncr = pow(logBase, exp);

	if (exp < 0)
		foundIncr = roundDec(foundIncr, -exp);

	let split = scaleMin;

	do {
		splits.push(split);
		split = roundDec(split + foundIncr, fixedDec.get(foundIncr));

		if (split >= foundIncr * logBase)
			foundIncr = split;

	} while (split <= scaleMax);

	return splits;
}

const RE_ALL   = /./;
const RE_12357 = /[12357]/;
const RE_125   = /[125]/;
const RE_1     = /1/;

function logAxisValsFilt(self, splits, axisIdx, foundSpace, foundIncr) {
	let axis = self.axes[axisIdx];
	let scaleKey = axis.scale;

	if (self.scales[scaleKey].log == 2)
		return splits;

	let valToPos = self.valToPos;

	let minSpace = axis._space;

	let _10 = valToPos(10, scaleKey);

	let re = (
		valToPos(9, scaleKey) - _10 >= minSpace ? RE_ALL :
		valToPos(7, scaleKey) - _10 >= minSpace ? RE_12357 :
		valToPos(5, scaleKey) - _10 >= minSpace ? RE_125 :
		RE_1
	);

	return splits.map(v => re.test(v) ? v : null);
}

function numSeriesVal(self, val) {
	return val == null ? "" : fmtNum(val);
}

const yAxisOpts = {
	show: true,
	scale: "y",
	space: 30,
	gap: 5,
	size: 50,
	labelSize: 30,
	labelFont,
	side: 3,
//	class: "y-vals",
//	incrs: numIncrs,
//	values: (vals, space) => vals,
//	filter: retArg1,
	grid,
	ticks,
	font,
	rotate: 0,
};

// takes stroke width
function ptDia(width, mult) {
	let dia = 3 + (width || 1) * 2;
	return roundDec(dia * mult, 3);
}

function seriesPoints(self, si) {
	const s = self.series[si];
	const dia = ptDia(s.width, pxRatio);
	let maxPts = self.bbox.width / (s.points.space * pxRatio);
	let idxs = self.series[0].idxs;
	return idxs[1] - idxs[0] <= maxPts;
}

function seriesFillTo(self, seriesIdx, dataMin, dataMax) {
	let scale = self.scales[self.series[seriesIdx].scale];
	return scale.distr == 3 ? scale.min : 0;
}

const ySeriesOpts = {
	scale: "y",
	auto: true,
	sorted: 0,
	show: true,
	band: false,
	spanGaps: false,
	isGap: (self, seriesIdx, dataIdx) => true,
	alpha: 1,
	points: {
		show: seriesPoints,
	//	stroke: "#000",
	//	fill: "#fff",
	//	width: 1,
	//	size: 10,
	},
//	label: "Value",
//	value: v => v,
	values: null,

	// internal caches
	min: inf,
	max: -inf,
	idxs: [],

	path: null,
	clip: null,
};

const xScaleOpts = {
	time: true,
	auto: true,
	distr: 1,
	log: 10,
	min: null,
	max: null,
};

const yScaleOpts = assign({}, xScaleOpts, {
	time: false,
});

const syncs = {};

function _sync(opts) {
	let clients = [];

	return {
		sub(client) {
			clients.push(client);
		},
		unsub(client) {
			clients = clients.filter(c => c != client);
		},
		pub(type, self, x, y, w, h, i) {
			if (clients.length > 1) {
				clients.forEach(client => {
					client != self && client.pub(type, self, x, y, w, h, i);
				});
			}
		}
	};
}

function setDefaults(d, xo, yo, initY) {
	let d2 = initY ? [d[0], d[1]].concat(d.slice(2)) : [d[0]].concat(d.slice(1));
	return d2.map((o, i) => setDefault(o, i, xo, yo));
}

function setDefault(o, i, xo, yo) {
	return assign({}, (i == 0 || o && o.side % 2 == 0 ? xo : yo), o);
}

function getValPct(val, scale) {
	return (
		scale.distr == 3
		? log10(val / scale.min) / log10(scale.max / scale.min)
		: (val - scale.min) / (scale.max - scale.min)
	);
}

function getYPos(val, scale, hgt, top) {
	let pctY = getValPct(val, scale);
	return top + (1 - pctY) * hgt;
}

function getXPos(val, scale, wid, lft) {
	let pctX = getValPct(val, scale);
	return lft + pctX * wid;
}

const nullMinMax = [null, null];

function snapNumX(self, dataMin, dataMax) {
	return dataMin == null ? nullMinMax : [dataMin, dataMax];
}

const snapTimeX = snapNumX;

// this ensures that non-temporal/numeric y-axes get multiple-snapped padding added above/below
// TODO: also account for incrs when snapping to ensure top of axis gets a tick & value
function snapNumY(self, dataMin, dataMax) {
	return dataMin == null ? nullMinMax : rangeNum(dataMin, dataMax, 0.1, true);
}

function snapLogY(self, dataMin, dataMax, scale) {
	return dataMin == null ? nullMinMax : rangeLog(dataMin, dataMax, self.scales[scale].log, false);
}

const snapLogX = snapLogY;

// dim is logical (getClientBoundingRect) pixels, not canvas pixels
function findIncr(min, max, incrs, dim, minSpace) {
	let pxPerUnit = dim / (max - min);

	let minDec = (""+floor(min)).length;

	for (var i = 0; i < incrs.length; i++) {
		let space = incrs[i] * pxPerUnit;

		let incrDec = incrs[i] < 10 ? fixedDec.get(incrs[i]) : 0;

		if (space >= minSpace && minDec + incrDec < 17)
			return [incrs[i], space];
	}

	return [0, 0];
}

function pxRatioFont(font) {
	let fontSize;
	font = font.replace(/(\d+)px/, (m, p1) => (fontSize = round(p1 * pxRatio)) + 'px');
	return [font, fontSize];
}

function uPlot(opts, data, then) {
	const self = {};

	let ready = false;
	self.status = 0;

	const root = self.root = placeDiv(UPLOT);

	if (opts.id != null)
		root.id = opts.id;

	addClass(root, opts.class);

	if (opts.title) {
		let title = placeDiv(TITLE, root);
		title.textContent = opts.title;
	}

	const can = placeTag("canvas");
	const ctx = self.ctx = can.getContext("2d");

	const wrap = placeDiv(WRAP, root);
	const under = placeDiv(UNDER, wrap);
	wrap.appendChild(can);
	const over = placeDiv(OVER, wrap);

	opts = copy(opts);

	(opts.plugins || []).forEach(p => {
		if (p.opts)
			opts = p.opts(self, opts) || opts;
	});



	const series  = self.series = setDefaults(opts.series || [], xSeriesOpts, ySeriesOpts, false);
	const axes    = self.axes   = setDefaults(opts.axes   || [], xAxisOpts,   yAxisOpts,    true);
	const scales  = self.scales = {};

	const xScaleKey = series[0].scale;

	function initScale(scaleKey) {
		let sc = scales[scaleKey];

		if (sc == null) {
			let scaleOpts = (opts.scales || EMPTY_OBJ)[scaleKey] || EMPTY_OBJ;

			if (scaleOpts.from != null) {
				// ensure parent is initialized
				initScale(scaleOpts.from);
				// dependent scales inherit
				scales[scaleKey] = assign({}, scales[scaleOpts.from], scaleOpts);
			}
			else {
				sc = scales[scaleKey] = assign({}, (scaleKey == xScaleKey ? xScaleOpts : yScaleOpts), scaleOpts);

				let isTime =  sc.time;
				let isLog  = sc.distr == 3;

				let rn = sc.range;

				if (scaleKey != xScaleKey && !isArr(rn) && isObj(rn)) {
					let cfg = rn;
					// this is similar to snapNumY
					rn = (self, dataMin, dataMax) => dataMin == null ? nullMinMax : rangeNum(dataMin, dataMax, cfg);
				}

				sc.range = fnOrSelf(rn || (isTime ? snapTimeX : scaleKey == xScaleKey ? (isLog ? snapLogX : snapNumX) : (isLog ? snapLogY : snapNumY)));

				sc.auto = fnOrSelf(sc.auto);
			}
		}
	}

	initScale("x");
	initScale("y");

	series.forEach((s, i) => {
		initScale(s.scale);
	});

	for (let k in opts.scales)
		initScale(k);

	const xScaleDistr = scales[xScaleKey].distr;

	const pendScales = {};

	// explicitly-set initial scales
	for (let k in scales) {
		let sc = scales[k];

		if (sc.min != null || sc.max != null)
			pendScales[k] = {min: sc.min, max: sc.max};
	}

	const gutters = self.gutters = assign({
		x: round(yAxisOpts.size / 2),
		y: round(xAxisOpts.size / 3),
		_x: null,
		_y: null,
	}, opts.gutters);

	gutters.x  = fnOrSelf(gutters.x);
	gutters.y  = fnOrSelf(gutters.y);
	gutters._x = gutters.x(self);
	gutters._y = gutters.y(self);

//	self.tz = opts.tz || Intl.DateTimeFormat().resolvedOptions().timeZone;
	const _tzDate  =  (opts.tzDate || (ts => new Date(ts * 1e3)));
	const _fmtDate =  (opts.fmtDate || fmtDate);

	const _timeAxisSplits =  timeAxisSplits(_tzDate);
	const _timeAxisVals   =  timeAxisVals(_tzDate, timeAxisStamps(_timeAxisStamps, _fmtDate));
	const _timeSeriesVal  =  timeSeriesVal(_tzDate, timeSeriesStamp(_timeSeriesStamp, _fmtDate));

	const legend     =  assign({show: true, live: true}, opts.legend);
	const showLegend =  legend.show;

	let legendEl;
	let legendRows = [];
	let legendCols;
	let multiValLegend = false;

	if (showLegend) {
		legendEl = placeTag("table", LEGEND, root);

		const getMultiVals = series[1] ? series[1].values : null;
		multiValLegend = getMultiVals != null;

		if (multiValLegend) {
			let head = placeTag("tr", LEGEND_THEAD, legendEl);
			placeTag("th", null, head);
			legendCols = getMultiVals(self, 1, 0);

			for (var key in legendCols)
				placeTag("th", LEGEND_LABEL, head).textContent = key;
		}
		else {
			legendCols = {_: 0};
			addClass(legendEl, LEGEND_INLINE);
			legend.live && addClass(legendEl, LEGEND_LIVE);
		}
	}

	function initLegendRow(s, i) {
		if (i == 0 && (multiValLegend || !legend.live))
			return null;

		let _row = [];

		let row = placeTag("tr", LEGEND_SERIES, legendEl, legendEl.childNodes[i]);

		addClass(row, s.class);

		if (!s.show)
			addClass(row, OFF);

		let label = placeTag("th", null, row);

		let indic = placeDiv(LEGEND_MARKER, label);
		indic.style.borderColor = s.width ? s.stroke : i > 0 && s.points.width ? s.points.stroke : null;
		indic.style.backgroundColor = s.fill || null;

		let text = placeDiv(LEGEND_LABEL, label);
		text.textContent = s.label;

		if (i > 0) {
			onMouse("click", label, e => {
				if ( cursor._lock)
					return;

				setSeries(series.indexOf(s), {show: !s.show},  syncOpts.setSeries);
			});

			if (cursorFocus) {
				onMouse(mouseenter, label, e => {
					if (cursor._lock)
						return;

					setSeries(series.indexOf(s), {focus: true}, syncOpts.setSeries);
				});
			}
		}

		for (var key in legendCols) {
			let v = placeTag("td", LEGEND_VALUE, row);
			v.textContent = "--";
			_row.push(v);
		}

		return _row;
	}

	const mouseListeners = new Map();

	function onMouse(ev, targ, fn) {
		const targListeners = mouseListeners.get(targ) || {};
		const listener = cursor.bind[ev](self, targ, fn);

		if (listener) {
			on(ev, targ, targListeners[ev] = listener);
			mouseListeners.set(targ, targListeners);
		}
	}

	function offMouse(ev, targ, fn) {
		const targListeners = mouseListeners.get(targ) || {};
		off(ev, targ, targListeners[ev]);
		targListeners[ev] = null;
	}

	let fullWidCss = 0;
	let fullHgtCss = 0;

	let plotWidCss = 0;
	let plotHgtCss = 0;

	// plot margins to account for axes
	let plotLftCss = 0;
	let plotTopCss = 0;

	let plotLft = 0;
	let plotTop = 0;
	let plotWid = 0;
	let plotHgt = 0;

	self.bbox = {};

	let shouldSetScales = false;
	let shouldSetSize = false;
	let shouldConvergeSize = false;
	let shouldSetCursor = false;
	let shouldSetLegend = false;

	function _setSize(width, height) {
		if (width != self.width || height != self.height)
			calcSize(width, height);

		resetYSeries(false);

		shouldConvergeSize = true;
		shouldSetSize = true;
		shouldSetCursor = true;
		shouldSetLegend = true;
		commit();
	}

	function calcSize(width, height) {
	//	log("calcSize()", arguments);

		self.width  = fullWidCss = plotWidCss = width;
		self.height = fullHgtCss = plotHgtCss = height;
		plotLftCss  = plotTopCss = 0;

		calcPlotRect();
		calcAxesRects();

		let bb = self.bbox;

		plotLft = bb[LEFT]   = incrRound(plotLftCss * pxRatio, 0.5);
		plotTop = bb[TOP]    = incrRound(plotTopCss * pxRatio, 0.5);
		plotWid = bb[WIDTH]  = incrRound(plotWidCss * pxRatio, 0.5);
		plotHgt = bb[HEIGHT] = incrRound(plotHgtCss * pxRatio, 0.5);
	}

	function convergeSize() {
		let converged = false;

		while (!converged) {
			let axesConverged = axesCalc();
			let guttersConverged = guttersCalc();

			converged = axesConverged && guttersConverged;

			if (!converged) {
				calcSize(self.width, self.height);
				shouldSetSize = true;
			}
		}
	}

	function setSize({width, height}) {
		_setSize(width, height);
	}

	self.setSize = setSize;

	// accumulate axis offsets, reduce canvas width
	function calcPlotRect() {
		// easements for edge labels
		let hasTopAxis = false;
		let hasBtmAxis = false;
		let hasRgtAxis = false;
		let hasLftAxis = false;

		axes.forEach((axis, i) => {
			if (axis.show && axis._show) {
				let {side, _size} = axis;
				let isVt = side % 2;
				let labelSize = axis.labelSize = (axis.label != null ? (axis.labelSize || 30) : 0);

				let fullSize = _size + labelSize;

				if (fullSize > 0) {
					if (isVt) {
						plotWidCss -= fullSize;

						if (side == 3) {
							plotLftCss += fullSize;
							hasLftAxis = true;
						}
						else
							hasRgtAxis = true;
					}
					else {
						plotHgtCss -= fullSize;

						if (side == 0) {
							plotTopCss += fullSize;
							hasTopAxis = true;
						}
						else
							hasBtmAxis = true;
					}
				}
			}
		});

		// hz gutters
		if (hasTopAxis || hasBtmAxis) {
			if (!hasRgtAxis)
				plotWidCss -= gutters._x;
			if (!hasLftAxis) {
				plotWidCss -= gutters._x;
				plotLftCss += gutters._x;
			}
		}

		// vt gutters
		if (hasLftAxis || hasRgtAxis) {
			if (!hasBtmAxis)
				plotHgtCss -= gutters._y;
			if (!hasTopAxis) {
				plotHgtCss -= gutters._y;
				plotTopCss += gutters._y;
			}
		}
	}

	function calcAxesRects() {
		// will accum +
		let off1 = plotLftCss + plotWidCss;
		let off2 = plotTopCss + plotHgtCss;
		// will accum -
		let off3 = plotLftCss;
		let off0 = plotTopCss;

		function incrOffset(side, size) {

			switch (side) {
				case 1: off1 += size; return off1 - size;
				case 2: off2 += size; return off2 - size;
				case 3: off3 -= size; return off3 + size;
				case 0: off0 -= size; return off0 + size;
			}
		}

		axes.forEach((axis, i) => {
			if (axis.show && axis._show) {
				let side = axis.side;

				axis._pos = incrOffset(side, axis._size);

				if (axis.label != null)
					axis._lpos = incrOffset(side, axis.labelSize);
			}
		});
	}

	const cursor =  (self.cursor = assign({}, cursorOpts, opts.cursor));

	 (cursor._lock = false);
	 (cursor.points.show = fnOrSelf(cursor.points.show));

	const focus = self.focus = assign({}, opts.focus || {alpha: 0.3},  cursor.focus);
	const cursorFocus =  focus.prox >= 0;

	// series-intersection markers
	let cursorPts = [null];

	function initCursorPt(s, si) {
		if (si > 0) {
			let pt = cursor.points.show(self, si);

			if (pt) {
				addClass(pt, CURSOR_PT);
				addClass(pt, s.class);
				trans(pt, -10, -10, plotWidCss, plotHgtCss);
				over.insertBefore(pt, cursorPts[si]);

				return pt;
			}
		}
	}

	function initSeries(s, i) {
		let isTime =  scales[s.scale].time;

		let sv = s.value;
		s.value = isTime ? (isStr(sv) ? timeSeriesVal(_tzDate, timeSeriesStamp(sv, _fmtDate)) : sv || _timeSeriesVal) : sv || numSeriesVal;
		s.label = s.label || (isTime ? timeSeriesLabel : numSeriesLabel);

		if (i > 0) {
			s.width = s.width == null ? 1 : s.width;
			s.paths = s.paths || ( buildPaths);
			s.fillTo = s.fillTo || seriesFillTo;
			let _ptDia = ptDia(s.width, 1);
			s.points = assign({}, {
				size: _ptDia,
				width: max(1, _ptDia * .2),
				stroke: s.stroke,
				space: _ptDia * 2,
			}, s.points);
			s.points.show = fnOrSelf(s.points.show);
			s._paths = null;
		}

		if (showLegend)
			legendRows.splice(i, 0, initLegendRow(s, i));

		if ( cursor.show) {
			let pt = initCursorPt(s, i);
			pt && cursorPts.splice(i, 0, pt);
		}
	}

	function addSeries(opts, si) {
		si = si == null ? series.length : si;

		opts = setDefault(opts, si, xSeriesOpts, ySeriesOpts);
		series.splice(si, 0, opts);
		initSeries(series[si], si);
	}

	self.addSeries = addSeries;

	function delSeries(i) {
		series.splice(i, 1);
		 showLegend && legendRows.splice(i, 1)[0][0].parentNode.remove();
		 cursorPts.length > 1 && cursorPts.splice(i, 1)[0].remove();

		// TODO: de-init no-longer-needed scales?
	}

	self.delSeries = delSeries;

	series.forEach(initSeries);

	function initAxis(axis, i) {
		axis._show = axis.show;

		if (axis.show) {
			let isVt = axis.side % 2;

			let sc = scales[axis.scale];

			// this can occur if all series specify non-default scales
			if (sc == null) {
				axis.scale = isVt ? series[1].scale : xScaleKey;
				sc = scales[axis.scale];
			}

			// also set defaults for incrs & values based on axis distr
			let isTime =  sc.time;

			axis.size   = fnOrSelf(axis.size);
			axis.space  = fnOrSelf(axis.space);
			axis.rotate = fnOrSelf(axis.rotate);
			axis.incrs  = fnOrSelf(axis.incrs  || (          sc.distr == 2 ? wholeIncrs : (isTime ? timeIncrs : numIncrs)));
			axis.splits = fnOrSelf(axis.splits || (isTime && sc.distr == 1 ? _timeAxisSplits : sc.distr == 3 ? logAxisSplits : numAxisSplits));

			let av = axis.values;
			axis.values = (
				isTime ? (
					isArr(av) ?
						timeAxisVals(_tzDate, timeAxisStamps(av, _fmtDate)) :
					isStr(av) ?
						timeAxisVal(_tzDate, av) :
					av || _timeAxisVals
				) : av || numAxisVals
			);

			axis.filter = fnOrSelf(axis.filter || (          sc.distr == 3 ? logAxisValsFilt : retArg1));

			axis.font      = pxRatioFont(axis.font);
			axis.labelFont = pxRatioFont(axis.labelFont);

			axis._size   = axis.size(self, null, i);

			axis._space  =
			axis._rotate =
			axis._incrs  =
			axis._found  =	// foundIncrSpace
			axis._splits =
			axis._values = null;
		}
	}

	// set axis defaults
	axes.forEach(initAxis);

	let dataLen;
	let dataIsGap;

	// rendered data window
	let i0 = null;
	let i1 = null;
	const idxs = series[0].idxs;

	let data0 = null;

	let viaAutoScaleX = false;

	function setData(_data, _resetScales) {
		if (!isArr(_data) && isObj(_data)) {
			dataIsGap = _data.isGap;
			_data = _data.data;
		}

		_data = _data || [];
		_data[0] = _data[0] || [];

		self.data = _data;
		data = _data.slice();
		data0 = data[0];
		dataLen = data0.length;

		if (xScaleDistr == 2)
			data[0] = data0.map((v, i) => i);

		resetYSeries(true);

		fire("setData");

		if (_resetScales !== false) {
			let xsc = scales[xScaleKey];

			if (xsc.auto(self, viaAutoScaleX))
				autoScaleX();
			else
				_setScale(xScaleKey, xsc.min, xsc.max);

			shouldSetCursor = true;
			shouldSetLegend = true;
			commit();
		}
	}

	self.setData = setData;

	function autoScaleX() {
		viaAutoScaleX = true;

		let _min, _max;

		if (dataLen > 0) {
			i0 = idxs[0] = 0;
			i1 = idxs[1] = dataLen - 1;

			_min = data[0][i0];
			_max = data[0][i1];

			if (xScaleDistr == 2) {
				_min = i0;
				_max = i1;
			}
			else if (dataLen == 1) {
				if (xScaleDistr == 3)
					[_min, _max] = rangeLog(_min, _min, scales[xScaleKey].log, false);
				else if (scales[xScaleKey].time)
					_max = _min + 86400;
				else
					[_min, _max] = rangeNum(_min, _max, 0.1, true);
			}
		}
		else {
			i0 = idxs[0] = _min = null;
			i1 = idxs[1] = _max = null;
		}

		_setScale(xScaleKey, _min, _max);

		viaAutoScaleX = false;
	}

	function setCtxStyle(stroke, width, dash, fill) {
		ctx.strokeStyle = stroke || transparent;
		ctx.lineWidth = width;
		ctx.lineJoin = "round";
		ctx.setLineDash(dash || []);
		ctx.fillStyle = fill || transparent;
	}

	function setScales() {
	//	log("setScales()", arguments);

		// wip scales
		let wipScales = copy(scales);

		for (let k in wipScales) {
			let wsc = wipScales[k];
			let psc = pendScales[k];

			if (psc != null && psc.min != null) {
				assign(wsc, psc);

				// explicitly setting the x-scale invalidates everything (acts as redraw)
				if (k == xScaleKey)
					resetYSeries(true);
			}
			else if (k != xScaleKey) {
				if (dataLen == 0 && wsc.from == null) {
					let minMax = wsc.range(self, null, null, k);
					wsc.min = minMax[0];
					wsc.max = minMax[1];
				}
				else {
					wsc.min = inf;
					wsc.max = -inf;
				}
			}
		}

		if (dataLen > 0) {
			// pre-range y-scales from y series' data values
			series.forEach((s, i) => {
				let k = s.scale;
				let wsc = wipScales[k];
				let psc = pendScales[k];

				if (i == 0) {
					let minMax = wsc.range(self, wsc.min, wsc.max, k);

					wsc.min = minMax[0];
					wsc.max = minMax[1];

					i0 = closestIdx(wsc.min, data[0]);
					i1 = closestIdx(wsc.max, data[0]);

					// closest indices can be outside of view
					if (data[0][i0] < wsc.min)
						i0++;
					if (data[0][i1] > wsc.max)
						i1--;

					s.min = data0[i0];
					s.max = data0[i1];
				}
				else if (s.show && s.auto && wsc.auto(self, viaAutoScaleX) && (psc == null || psc.min == null)) {
					// only run getMinMax() for invalidated series data, else reuse
					let minMax = s.min == null ? getMinMax(data[i], i0, i1, s.sorted) : [s.min, s.max];

					// initial min/max
					wsc.min = min(wsc.min, s.min = minMax[0]);
					wsc.max = max(wsc.max, s.max = minMax[1]);
				}

				s.idxs[0] = i0;
				s.idxs[1] = i1;
			});

			// range independent scales
			for (let k in wipScales) {
				let wsc = wipScales[k];
				let psc = pendScales[k];

				if (wsc.from == null && (psc == null || psc.min == null)) {
					let minMax = wsc.range(
						self,
						wsc.min ==  inf ? null : wsc.min,
						wsc.max == -inf ? null : wsc.max,
						k
					);
					wsc.min = minMax[0];
					wsc.max = minMax[1];
				}
			}
		}

		// range dependent scales
		for (let k in wipScales) {
			let wsc = wipScales[k];

			if (wsc.from != null) {
				let base = wipScales[wsc.from];
				let minMax = wsc.range(self, base.min, base.max, k);
				wsc.min = minMax[0];
				wsc.max = minMax[1];
			}
		}

		let changed = {};
		let anyChanged = false;

		for (let k in wipScales) {
			let wsc = wipScales[k];
			let sc = scales[k];

			if (sc.min != wsc.min || sc.max != wsc.max) {
				sc.min = wsc.min;
				sc.max = wsc.max;
				changed[k] = anyChanged = true;
			}
		}

		if (anyChanged) {
			// invalidate paths of all series on changed scales
			series.forEach(s => {
				if (changed[s.scale])
					s._paths = null;
			});

			for (let k in changed) {
				shouldConvergeSize = true;
				fire("setScale", k);
			}

			if ( cursor.show)
				shouldSetCursor = true;
		}

		for (let k in pendScales)
			pendScales[k] = null;
	}

	// TODO: drawWrap(si, drawPoints) (save, restore, translate, clip)

	function drawPoints(si) {
	//	log("drawPoints()", arguments);

		let s = series[si];
		let p = s.points;

		const width = roundDec(p.width * pxRatio, 3);
		const offset = (width % 2) / 2;
		const isStroked = p.width > 0;

		let rad = (p.size - p.width) / 2 * pxRatio;
		let dia = roundDec(rad * 2, 3);

		ctx.translate(offset, offset);

		ctx.save();

		ctx.beginPath();
		ctx.rect(
			plotLft - dia,
			plotTop - dia,
			plotWid + dia * 2,
			plotHgt + dia * 2,
		);
		ctx.clip();

		ctx.globalAlpha = s.alpha;

		const path = new Path2D();

		for (let pi = i0; pi <= i1; pi++) {
			if (data[si][pi] != null) {
				let x = round(getXPos(data[0][pi],  scales[xScaleKey], plotWid, plotLft));
				let y = round(getYPos(data[si][pi], scales[s.scale],   plotHgt, plotTop));

				path.moveTo(x + rad, y);
				path.arc(x, y, rad, 0, PI * 2);
			}
		}

		setCtxStyle(
			p.stroke,
			width,
			null,
			p.fill || (isStroked ? "#fff" : s.stroke),
		);

		ctx.fill(path);
		isStroked && ctx.stroke(path);

		ctx.globalAlpha = 1;

		ctx.restore();

		ctx.translate(-offset, -offset);
	}

	// grabs the nearest indices with y data outside of x-scale limits
	function getOuterIdxs(ydata) {
		let _i0 = clamp(i0 - 1, 0, dataLen - 1);
		let _i1 = clamp(i1 + 1, 0, dataLen - 1);

		while (ydata[_i0] == null && _i0 > 0)
			_i0--;

		while (ydata[_i1] == null && _i1 < dataLen - 1)
			_i1++;

		return [_i0, _i1];
	}

	let dir = 1;

	function drawSeries() {
		// path building loop must be before draw loop to ensure that all bands are fully constructed
		series.forEach((s, i) => {
			if (i > 0 && s.show && s._paths == null) {
				let _idxs = getOuterIdxs(data[i]);
				s._paths = s.paths(self, i, _idxs[0], _idxs[1]);
			}
		});

		series.forEach((s, i) => {
			if (i > 0 && s.show) {
				if (s._paths)
					 drawPath(i);

				if (s.points.show(self, i, i0, i1))
					 drawPoints(i);

				fire("drawSeries", i);
			}
		});
	}

	function drawPath(si) {
		const s = series[si];

		if (dir == 1) {
			const { stroke, fill, clip } = s._paths;
			const width = roundDec(s[WIDTH] * pxRatio, 3);
			const offset = (width % 2) / 2;

			setCtxStyle(s.stroke, width, s.dash, s.fill);

			ctx.globalAlpha = s.alpha;

			ctx.translate(offset, offset);

			ctx.save();

			let lft = plotLft,
				top = plotTop,
				wid = plotWid,
				hgt = plotHgt;

			let halfWid = width * pxRatio / 2;

			if (s.min == 0)
				hgt += halfWid;

			if (s.max == 0) {
				top -= halfWid;
				hgt += halfWid;
			}

			ctx.beginPath();
			ctx.rect(lft, top, wid, hgt);
			ctx.clip();

			if (clip != null)
				ctx.clip(clip);

			if (s.band) {
				ctx.fill(stroke);
				width && ctx.stroke(stroke);
			}
			else {
				width && ctx.stroke(stroke);

				if (s.fill != null)
					ctx.fill(fill);
			}

			ctx.restore();

			ctx.translate(-offset, -offset);

			ctx.globalAlpha = 1;
		}

		if (s.band)
			dir *= -1;
	}

	function buildClip(is, gaps, nullHead, nullTail) {
		let s = series[is];

		let clip = null;

		// create clip path (invert gaps and non-gaps)
		if (gaps.length > 0 && !s.spanGaps) {
			clip = new Path2D();

			let prevGapEnd = plotLft;

			for (let i = 0; i < gaps.length; i++) {
				let g = gaps[i];

				clip.rect(prevGapEnd, plotTop, g[0] - prevGapEnd, plotTop + plotHgt);

				prevGapEnd = g[1];
			}

			clip.rect(prevGapEnd, plotTop, plotLft + plotWid - prevGapEnd, plotTop + plotHgt);
		}

		return clip;
	}

	function addGap(gaps, fromX, toX) {
		if (toX > fromX) {
			let prevGap = gaps[gaps.length - 1];

			if (prevGap && prevGap[0] == fromX)			// TODO: gaps must be encoded at stroke widths?
				prevGap[1] = toX;
			else
				gaps.push([fromX, toX]);
		}
	}

	function nonNullIdx(data, _i0, _i1, dir) {
		for (let i = dir == 1 ? _i0 : _i1; i >= _i0 && i <= _i1; i += dir) {
			if (data[i] != null)
				return i;
		}

		return -1;
	}

	function buildPaths(self, is, _i0, _i1) {
		const s = series[is];
		const isGap = dataIsGap || s.isGap;

		const xdata  = data[0];
		const ydata  = data[is];
		const scaleX = scales[xScaleKey];
		const scaleY = scales[s.scale];

		const _paths = dir == 1 ? {stroke: new Path2D(), fill: null, clip: null} : series[is-1]._paths;
		const stroke = _paths.stroke;
		const width = roundDec(s[WIDTH] * pxRatio, 3);

		let minY = inf,
			maxY = -inf,
			outY, outX;

		// todo: don't build gaps on dir = -1 pass
		let gaps = [];

		let accX = round(getXPos(xdata[dir == 1 ? _i0 : _i1], scaleX, plotWid, plotLft));
		let accGaps = false;

		// data edges
		let lftIdx = nonNullIdx(ydata, _i0, _i1, 1);
		let rgtIdx = nonNullIdx(ydata, _i0, _i1, -1);
		let lftX = incrRound(getXPos(xdata[lftIdx], scaleX, plotWid, plotLft), 0.5);
		let rgtX = incrRound(getXPos(xdata[rgtIdx], scaleX, plotWid, plotLft), 0.5);

		if (lftX > plotLft)
			addGap(gaps, plotLft, lftX);

		// the moves the shape edge outside the canvas so stroke doesnt bleed in
		if (s.band && dir == 1)
			stroke.lineTo(lftX - width * 2, round(getYPos(ydata[_i0], scaleY, plotHgt, plotTop)));

		for (let i = dir == 1 ? _i0 : _i1; i >= _i0 && i <= _i1; i += dir) {
			let x = round(getXPos(xdata[i], scaleX, plotWid, plotLft));

			if (x == accX) {
				if (ydata[i] != null) {
					outY = round(getYPos(ydata[i], scaleY, plotHgt, plotTop));
					minY = min(outY, minY);
					maxY = max(outY, maxY);
				}
				else if (!accGaps && isGap(self, is, i))
					accGaps = true;
			}
			else {
				let _addGap = false;

				if (minY != inf) {
					stroke.lineTo(accX, minY);
					stroke.lineTo(accX, maxY);
					stroke.lineTo(accX, outY);
					outX = accX;
				}
				else if (accGaps) {
					_addGap = true;
					accGaps = false;
				}

				if (ydata[i] != null) {
					outY = round(getYPos(ydata[i], scaleY, plotHgt, plotTop));
					stroke.lineTo(x, outY);
					minY = maxY = outY;

					// prior pixel can have data but still start a gap if ends with null
					if (x - accX > 1 && ydata[i-1] == null && isGap(self, is, i-1))
						_addGap = true;
				}
				else {
					minY = inf;
					maxY = -inf;

					if (!accGaps && isGap(self, is, i))
						accGaps = true;
				}

				_addGap && addGap(gaps, outX, x);

				accX = x;
			}
		}

		if (rgtX < plotLft + plotWid)
			addGap(gaps, rgtX, plotLft + plotWid);

		if (s.band) {
			let _x, _iy, ydata2;

			// the moves the shape edge outside the canvas so stroke doesnt bleed in
			if (dir == 1) {
				_x = rgtX + width * 2;
				_iy = rgtIdx;
				ydata2 = data[is + 1];
			}
			else {
				_x = lftX - width * 2;
				_iy = lftIdx;
				ydata2 = data[is - 1];
			}

			stroke.lineTo(_x, round(getYPos(ydata[_iy],  scaleY, plotHgt, plotTop)));
			stroke.lineTo(_x, round(getYPos(ydata2[_iy], scaleY, plotHgt, plotTop)));
		}

		if (dir == 1) {
			_paths.clip = buildClip(is, gaps, ydata[_i0] == null, ydata[_i1] == null);

			if (s.fill != null) {
				let fill = _paths.fill = new Path2D(stroke);

				let fillTo = round(getYPos(s.fillTo(self, is, s.min, s.max), scaleY, plotHgt, plotTop));
				fill.lineTo(rgtX, fillTo);
				fill.lineTo(lftX, fillTo);
			}
		}

		if (s.band)
			dir *= -1;

		return _paths;
	}

	self.paths = buildPaths;

	function getIncrSpace(axisIdx, min, max, fullDim) {
		let axis = axes[axisIdx];

		let incrSpace;

		if (fullDim <= 0)
			incrSpace = [0, 0];
		else {
			let minSpace = axis._space = axis.space(self, axisIdx, min, max, fullDim);
			let incrs    = axis._incrs = axis.incrs(self, axisIdx, min, max, fullDim, minSpace);
			incrSpace    = axis._found = findIncr(min, max, incrs, fullDim, minSpace);
		}

		return incrSpace;
	}

	function drawOrthoLines(offs, filts, ori, side, pos0, len, width, stroke, dash) {
		let offset = (width % 2) / 2;

		ctx.translate(offset, offset);

		setCtxStyle(stroke, width, dash);

		ctx.beginPath();

		let x0, y0, x1, y1, pos1 = pos0 + (side == 0 || side == 3 ? -len : len);

		if (ori == 0) {
			y0 = pos0;
			y1 = pos1;
		}
		else {
			x0 = pos0;
			x1 = pos1;
		}

		offs.forEach((off, i) => {
			if (filts[i] == null)
				return;

			if (ori == 0)
				x0 = x1 = off;
			else
				y0 = y1 = off;

			ctx.moveTo(x0, y0);
			ctx.lineTo(x1, y1);
		});

		ctx.stroke();

		ctx.translate(-offset, -offset);
	}

	function axesCalc() {
	//	log("axesCalc()", arguments);

		let converged = true;

		axes.forEach((axis, i) => {
			if (!axis.show)
				return;

			let scale = scales[axis.scale];

			if (scale.min == null) {
				if (axis._show) {
					converged = false;
					axis._show = false;
					resetYSeries(false);
				}
				return;
			}
			else {
				if (!axis._show) {
					converged = false;
					axis._show = true;
					resetYSeries(false);
				}
			}

			let side = axis.side;
			let ori = side % 2;

			let {min, max} = scale;		// 		// should this toggle them ._show = false

			let [_incr, _space] = getIncrSpace(i, min, max, ori == 0 ? plotWidCss : plotHgtCss);

			if (_space == 0)
				return;

			// if we're using index positions, force first tick to match passed index
			let forceMin = scale.distr == 2;

			let _splits = axis._splits = axis.splits(self, i, min, max, _incr, _space, forceMin);

			// tick labels
			// BOO this assumes a specific data/series
			let splits = scale.distr == 2 ? _splits.map(i => data0[i]) : _splits;
			let incr   = scale.distr == 2 ? data0[_splits[1]] - data0[_splits[0]] : _incr;

			let values = axis._values  = axis.values(self, axis.filter(self, splits, i, _space, incr), i, _space, incr);

			// rotating of labels only supported on bottom x axis
			axis._rotate = side == 2 ? axis.rotate(self, values, i, _space) : 0;

			let oldSize = axis._size;

			axis._size = axis.size(self, values, i);

			if (oldSize != null && axis._size != oldSize)			// ready && ?
				converged = false;
		});

		return converged;
	}

	function guttersCalc() {
		let converged = true;

		let {_x, _y} = gutters;

		gutters._x = gutters.x(self);
		gutters._y = gutters.y(self);

		if (gutters._x != _x || gutters._y != _y)
			converged = false;

		return converged;
	}

	function drawAxesGrid() {
		axes.forEach((axis, i) => {
			if (!axis.show || !axis._show)
				return;

			let scale = scales[axis.scale];
			let side = axis.side;
			let ori = side % 2;

			let getPos  = ori == 0 ? getXPos : getYPos;
			let plotDim = ori == 0 ? plotWid : plotHgt;
			let plotOff = ori == 0 ? plotLft : plotTop;

			let axisGap  = round(axis.gap * pxRatio);

			let ticks = axis.ticks;
			let tickSize = ticks.show ? round(ticks.size * pxRatio) : 0;

			let [_incr, _space] = axis._found;
			let _splits = axis._splits;

			// tick labels
			// BOO this assumes a specific data/series
			let splits = scale.distr == 2 ? _splits.map(i => data0[i]) : _splits;
			let incr   = scale.distr == 2 ? data0[_splits[1]] - data0[_splits[0]] : _incr;

			// rotating of labels only supported on bottom x axis
			let angle = axis._rotate * -PI/180;

			let basePos  = round(axis._pos * pxRatio);
			let shiftAmt = tickSize + axisGap;
			let shiftDir = ori == 0 && side == 0 || ori == 1 && side == 3 ? -1 : 1;
			let finalPos = basePos + shiftAmt * shiftDir;
			let y        = ori == 0 ? finalPos : 0;
			let x        = ori == 1 ? finalPos : 0;

			ctx.font         = axis.font[0];
			ctx.fillStyle    = axis.stroke || hexBlack;									// rgba?
			ctx.textAlign    = axis.align == 1 ? LEFT :
			                   axis.align == 2 ? RIGHT :
			                   angle > 0 ? LEFT :
			                   angle < 0 ? RIGHT :
			                   ori == 0 ? "center" : side == 3 ? RIGHT : LEFT;
			ctx.textBaseline = angle ||
			                   ori == 1 ? "middle" : side == 2 ? TOP   : BOTTOM;

			let lineHeight   = axis.font[1] * lineMult;

			let canOffs = _splits.map(val => round(getPos(val, scale, plotDim, plotOff)));

			axis._values.forEach((val, i) => {
				if (val == null)
					return;

				if (ori == 0)
					x = canOffs[i];
				else
					y = canOffs[i];

				(""+val).split(/\n/gm).forEach((text, j) => {
					if (angle) {
						ctx.save();
						ctx.translate(x, y + j * lineHeight);
						ctx.rotate(angle);
						ctx.fillText(text, 0, 0);
						ctx.restore();
					}
					else
						ctx.fillText(text, x, y + j * lineHeight);
				});
			});

			// axis label
			if (axis.label) {
				ctx.save();

				let baseLpos = round(axis._lpos * pxRatio);

				if (ori == 1) {
					x = y = 0;

					ctx.translate(
						baseLpos,
						round(plotTop + plotHgt / 2),
					);
					ctx.rotate((side == 3 ? -PI : PI) / 2);

				}
				else {
					x = round(plotLft + plotWid / 2);
					y = baseLpos;
				}

				ctx.font         = axis.labelFont[0];
			//	ctx.fillStyle    = axis.labelStroke || hexBlack;						// rgba?
				ctx.textAlign    = "center";
				ctx.textBaseline = side == 2 ? TOP : BOTTOM;

				ctx.fillText(axis.label, x, y);

				ctx.restore();
			}

			// ticks
			if (ticks.show) {
				drawOrthoLines(
					canOffs,
					ticks.filter(self, splits, i, _space, incr),
					ori,
					side,
					basePos,
					tickSize,
					roundDec(ticks[WIDTH] * pxRatio, 3),
					ticks.stroke,
				);
			}

			// grid
			let grid = axis.grid;

			if (grid.show) {
				drawOrthoLines(
					canOffs,
					grid.filter(self, splits, i, _space, incr),
					ori,
					ori == 0 ? 2 : 1,
					ori == 0 ? plotTop : plotLft,
					ori == 0 ? plotHgt : plotWid,
					roundDec(grid[WIDTH] * pxRatio, 3),
					grid.stroke,
					grid.dash,
				);
			}
		});

		fire("drawAxes");
	}

	function resetYSeries(minMax) {
	//	log("resetYSeries()", arguments);

		series.forEach((s, i) => {
			if (i > 0) {
				s._paths = null;

				if (minMax) {
					s.min = null;
					s.max = null;
				}
			}
		});
	}

	let queuedCommit = false;

	// could do rAF instead of microTask, or Promose.resolve().then()
	function commit() {
		if (!queuedCommit) {
			microTask(_commit);
			queuedCommit = true;
		}
	}

	function _commit() {
	//	log("_commit()", arguments);

		if (shouldSetScales) {
			setScales();
			shouldSetScales = false;
		}

		if (shouldConvergeSize) {
			convergeSize();
			shouldConvergeSize = false;
		}

		if (shouldSetSize) {
			setStylePx(under, LEFT,   plotLftCss);
			setStylePx(under, TOP,    plotTopCss);
			setStylePx(under, WIDTH,  plotWidCss);
			setStylePx(under, HEIGHT, plotHgtCss);

			setStylePx(over, LEFT,    plotLftCss);
			setStylePx(over, TOP,     plotTopCss);
			setStylePx(over, WIDTH,   plotWidCss);
			setStylePx(over, HEIGHT,  plotHgtCss);

			setStylePx(wrap, WIDTH,   fullWidCss);
			setStylePx(wrap, HEIGHT,  fullHgtCss);

			can[WIDTH]  = round(fullWidCss * pxRatio);
			can[HEIGHT] = round(fullHgtCss * pxRatio);

			syncRect();

			fire("setSize");

			shouldSetSize = false;
		}

	//	if (shouldSetSelect) {
		// TODO: update .u-select metrics (if visible)
		//	setStylePx(selectDiv, TOP, select[TOP] = 0);
		//	setStylePx(selectDiv, LEFT, select[LEFT] = 0);
		//	setStylePx(selectDiv, WIDTH, select[WIDTH] = 0);
		//	setStylePx(selectDiv, HEIGHT, select[HEIGHT] = 0);
		//	shouldSetSelect = false;
	//	}

		if ( cursor.show && shouldSetCursor) {
			updateCursor();
			shouldSetCursor = false;
		}

	//	if (true && legend.show && legend.live && shouldSetLegend) {}

		if (fullWidCss > 0 && fullHgtCss > 0) {
			ctx.clearRect(0, 0, can[WIDTH], can[HEIGHT]);
			fire("drawClear");
			drawAxesGrid();
			dataLen > 0 && drawSeries();
			fire("draw");
		}

		if (!ready) {
			ready = true;
			self.status = 1;

			fire("ready");
		}

		queuedCommit = false;
	}

	self.redraw = rebuildPaths => {
		if (rebuildPaths !== false)
			_setScale(xScaleKey, scales[xScaleKey].min, scales[xScaleKey].max);
		else
			commit();
	};

	// redraw() => setScale('x', scales.x.min, scales.x.max);

	// explicit, never re-ranged (is this actually true? for x and y)
	function setScale(key, opts) {
		let sc = scales[key];

		if (sc.from == null) {
			if (dataLen == 0) {
				let minMax = sc.range(self, opts.min, opts.max, key);
				opts.min = minMax[0];
				opts.max = minMax[1];
			}

			if (dataLen > 1 && opts.min != null && opts.max != null && opts.max - opts.min < 1e-16)
				return;

			if (key == xScaleKey) {
				if (sc.distr == 2 && dataLen > 0) {
					opts.min = closestIdx(opts.min, data[0]);
					opts.max = closestIdx(opts.max, data[0]);
				}
			}

		//	log("setScale()", arguments);

			pendScales[key] = opts;

			shouldSetScales = true;
			commit();
		}
	}

	self.setScale = setScale;

//	INTERACTION

	let vt;
	let hz;

	// starting position before cursor.move
	let rawMouseLeft0;
	let rawMouseTop0;

	// starting position
	let mouseLeft0;
	let mouseTop0;

	// current position before cursor.move
	let rawMouseLeft1;
	let rawMouseTop1;

	// current position
	let mouseLeft1;
	let mouseTop1;

	let dragging = false;

	const drag =  cursor.drag;

	let dragX =  drag.x;
	let dragY =  drag.y;

	if ( cursor.show) {
		if (cursor.x) {
			mouseLeft1 = cursor.left;
			vt = placeDiv(CURSOR_X, over);
		}

		if (cursor.y) {
			mouseTop1 = cursor.top;
			hz = placeDiv(CURSOR_Y, over);
		}
	}

	const select = self.select = assign({
		show:   true,
		over:   true,
		left:	0,
		width:	0,
		top:	0,
		height:	0,
	}, opts.select);

	const selectDiv = select.show ? placeDiv(SELECT, select.over ? over : under) : null;

	function setSelect(opts, _fire) {
		if (select.show) {
			for (let prop in opts)
				setStylePx(selectDiv, prop, select[prop] = opts[prop]);

			_fire !== false && fire("setSelect");
		}
	}

	self.setSelect = setSelect;

	function toggleDOM(i, onOff) {
		let s = series[i];
		let label = showLegend ? legendRows[i][0].parentNode : null;

		if (s.show)
			label && remClass(label, OFF);
		else {
			label && addClass(label, OFF);
			 cursorPts.length > 1 && trans(cursorPts[i], -10, -10, plotWidCss, plotHgtCss);
		}
	}

	function _setScale(key, min, max) {
		setScale(key, {min, max});
	}

	function setSeries(i, opts, pub) {
	//	log("setSeries()", arguments);

		let s = series[i];

		// will this cause redundant commit() if both show and focus are set?
		if (opts.focus != null)
			setFocus(i);

		if (opts.show != null) {
			s.show = opts.show;
			 toggleDOM(i, opts.show);

			if (s.band) {
				// not super robust, will break if two bands are adjacent
				let ip = series[i+1] && series[i+1].band ? i+1 : i-1;
				series[ip].show = s.show;
				 toggleDOM(ip, opts.show);
			}

			_setScale(s.scale, null, null);
			commit();
		}

		fire("setSeries", i, opts);

		 pub && sync.pub("setSeries", self, i, opts);
	}

	self.setSeries = setSeries;

	function _alpha(i, value) {
		series[i].alpha = value;

		if ( cursor.show && cursorPts[i])
			cursorPts[i].style.opacity = value;

		if ( showLegend && legendRows[i])
			legendRows[i][0].parentNode.style.opacity = value;
	}

	function _setAlpha(i, value) {
		let s = series[i];

		_alpha(i, value);

		if (s.band) {
			// not super robust, will break if two bands are adjacent
			let ip = series[i+1].band ? i+1 : i-1;
			_alpha(ip, value);
		}
	}

	// y-distance
	let closestDist;
	let closestSeries;
	let focusedSeries;

	function setFocus(i) {
		if (i != focusedSeries) {
		//	log("setFocus()", arguments);

			series.forEach((s, i2) => {
				_setAlpha(i2, i == null || i2 == 0 || i2 == i ? 1 : focus.alpha);
			});

			focusedSeries = i;
			commit();
		}
	}

	if (showLegend && cursorFocus) {
		on(mouseleave, legendEl, e => {
			if (cursor._lock)
				return;
			setSeries(null, {focus: false}, syncOpts.setSeries);
			updateCursor();
		});
	}

	function scaleValueAtPos(pos, scale) {
		let dim = plotWidCss;

		if (scale != xScaleKey) {
			dim = plotHgtCss;
			pos = dim - pos;
		}

		let pct = pos / dim;

		let sc = scales[scale],
			_min = sc.min,
			_max = sc.max;

		if (sc.distr == 3) {
			_min = log10(_min);
			_max = log10(_max);
			return pow(10, _min + (_max - _min) * pct);
		}
		else
			return _min + (_max - _min) * pct;
	}

	function closestIdxFromXpos(pos) {
		let v = scaleValueAtPos(pos, xScaleKey);
		return closestIdx(v, data[0], i0, i1);
	}

	self.valToIdx = val => closestIdx(val, data[0]);
	self.posToIdx = closestIdxFromXpos;
	self.posToVal = scaleValueAtPos;
	self.valToPos = (val, scale, can) => (
		scale == xScaleKey ?
		getXPos(val, scales[scale],
			can ? plotWid : plotWidCss,
			can ? plotLft : 0,
		) :
		getYPos(val, scales[scale],
			can ? plotHgt : plotHgtCss,
			can ? plotTop : 0,
		)
	);

	// defers calling expensive functions
	function batch(fn) {
		fn(self);
		commit();
	}

	self.batch = batch;

	 (self.setCursor = opts => {
		mouseLeft1 = opts.left;
		mouseTop1 = opts.top;
	//	assign(cursor, opts);
		updateCursor();
	});

	let cursorRaf = 0;

	function updateCursor(ts, src) {
	//	ts == null && log("updateCursor()", arguments);

		cursorRaf = 0;

		rawMouseLeft1 = mouseLeft1;
		rawMouseTop1 = mouseTop1;

		[mouseLeft1, mouseTop1] = cursor.move(self, mouseLeft1, mouseTop1);

		if (cursor.show) {
			cursor.x && trans(vt, round(mouseLeft1), 0, plotWidCss, plotHgtCss);
			cursor.y && trans(hz, 0, round(mouseTop1), plotWidCss, plotHgtCss);
		}

		let idx;

		// when zooming to an x scale range between datapoints the binary search
		// for nearest min/max indices results in this condition. cheap hack :D
		let noDataInRange = i0 > i1;

		closestDist = inf;

		// if cursor hidden, hide points & clear legend vals
		if (mouseLeft1 < 0 || dataLen == 0 || noDataInRange) {
			idx = null;

			for (let i = 0; i < series.length; i++) {
				if (i > 0) {
					 cursorPts.length > 1 && trans(cursorPts[i], -10, -10, plotWidCss, plotHgtCss);
				}

				if (showLegend && legend.live) {
					if (i == 0 && multiValLegend)
						continue;

					for (let j = 0; j < legendRows[i].length; j++)
						legendRows[i][j][firstChild].nodeValue = '--';
				}
			}

			if (cursorFocus)
				setSeries(null, {focus: true}, syncOpts.setSeries);
		}
		else {
		//	let pctY = 1 - (y / rect[HEIGHT]);

			let valAtPos = scaleValueAtPos(mouseLeft1, xScaleKey);

			idx = closestIdx(valAtPos, data[0], i0, i1);

			let scX = scales[xScaleKey];

			let xPos = roundDec(getXPos(data[0][idx], scX, plotWidCss, 0), 3);

			for (let i = 0; i < series.length; i++) {
				let s = series[i];

				let idx2  = cursor.dataIdx(self, i, idx, valAtPos);
				let xPos2 = idx2 == idx ? xPos : roundDec(getXPos(data[0][idx2], scX, plotWidCss, 0), 3);

				if (i > 0 && s.show) {
					let valAtIdx = data[i][idx2];

					let yPos = valAtIdx == null ? -10 : roundDec(getYPos(valAtIdx, scales[s.scale], plotHgtCss, 0), 3);

					if (yPos > 0) {
						let dist = abs(yPos - mouseTop1);

						if (dist <= closestDist) {
							closestDist = dist;
							closestSeries = i;
						}
					}

					 cursorPts.length > 1 && trans(cursorPts[i], xPos2, yPos, plotWidCss, plotHgtCss);
				}

				if (showLegend && legend.live) {
					if ((idx2 == cursor.idx && !shouldSetLegend) || i == 0 && multiValLegend)
						continue;

					let src = i == 0 && xScaleDistr == 2 ? data0 : data[i];

					let vals = multiValLegend ? s.values(self, i, idx2) : {_: s.value(self, src[idx2], i, idx2)};

					let j = 0;

					for (let k in vals)
						legendRows[i][j++][firstChild].nodeValue = vals[k];
				}
			}

			shouldSetLegend = false;
		}

		// nit: cursor.drag.setSelect is assumed always true
		if (select.show && dragging) {
			if (src != null) {
				let [xKey, yKey] = syncOpts.scales;

				// match the dragX/dragY implicitness/explicitness of src
				let sdrag = src.cursor.drag;
				dragX = sdrag._x;
				dragY = sdrag._y;

				if (xKey) {
					let sc = scales[xKey];
					let srcLeft = src.posToVal(src.select[LEFT], xKey);
					let srcRight = src.posToVal(src.select[LEFT] + src.select[WIDTH], xKey);

					select[LEFT] = getXPos(srcLeft, sc, plotWidCss, 0);
					select[WIDTH] = abs(select[LEFT] - getXPos(srcRight, sc, plotWidCss, 0));

					setStylePx(selectDiv, LEFT, select[LEFT]);
					setStylePx(selectDiv, WIDTH, select[WIDTH]);

					if (!yKey) {
						setStylePx(selectDiv, TOP, select[TOP] = 0);
						setStylePx(selectDiv, HEIGHT, select[HEIGHT] = plotHgtCss);
					}
				}

				if (yKey) {
					let sc = scales[yKey];
					let srcTop = src.posToVal(src.select[TOP], yKey);
					let srcBottom = src.posToVal(src.select[TOP] + src.select[HEIGHT], yKey);

					select[TOP] = getYPos(srcTop, sc, plotHgtCss, 0);
					select[HEIGHT] = abs(select[TOP] - getYPos(srcBottom, sc, plotHgtCss, 0));

					setStylePx(selectDiv, TOP, select[TOP]);
					setStylePx(selectDiv, HEIGHT, select[HEIGHT]);

					if (!xKey) {
						setStylePx(selectDiv, LEFT, select[LEFT] = 0);
						setStylePx(selectDiv, WIDTH, select[WIDTH] = plotWidCss);
					}
				}
			}
			else {
				let rawDX = abs(rawMouseLeft1 - rawMouseLeft0);
				let rawDY = abs(rawMouseTop1 - rawMouseTop0);

				dragX = drag.x && rawDX >= drag.dist;
				dragY = drag.y && rawDY >= drag.dist;

				let uni = drag.uni;

				if (uni != null) {
					// only calc drag status if they pass the dist thresh
					if (dragX && dragY) {
						dragX = rawDX >= uni;
						dragY = rawDY >= uni;

						// force unidirectionality when both are under uni limit
						if (!dragX && !dragY) {
							if (rawDY > rawDX)
								dragY = true;
							else
								dragX = true;
						}
					}
				}
				else if (drag.x && drag.y && (dragX || dragY))
					// if omni with no uni then both dragX / dragY should be true if either is true
					dragX = dragY = true;

				if (dragX) {
					let minX = min(mouseLeft0, mouseLeft1);
					let dx = abs(mouseLeft1 - mouseLeft0);

					setStylePx(selectDiv, LEFT,  select[LEFT] = minX);
					setStylePx(selectDiv, WIDTH, select[WIDTH] = dx);

					if (!dragY) {
						setStylePx(selectDiv, TOP, select[TOP] = 0);
						setStylePx(selectDiv, HEIGHT, select[HEIGHT] = plotHgtCss);
					}
				}

				if (dragY) {
					let minY = min(mouseTop0, mouseTop1);
					let dy = abs(mouseTop1 - mouseTop0);

					setStylePx(selectDiv, TOP,    select[TOP] = minY);
					setStylePx(selectDiv, HEIGHT, select[HEIGHT] = dy);

					if (!dragX) {
						setStylePx(selectDiv, LEFT, select[LEFT] = 0);
						setStylePx(selectDiv, WIDTH, select[WIDTH] = plotWidCss);
					}
				}

				if (!dragX && !dragY) {
					// the drag didn't pass the dist requirement
					setStylePx(selectDiv, HEIGHT, select[HEIGHT] = 0);
					setStylePx(selectDiv, WIDTH,  select[WIDTH]  = 0);
				}
			}
		}

		cursor.idx = idx;
		cursor.left = mouseLeft1;
		cursor.top = mouseTop1;
		drag._x = dragX;
		drag._y = dragY;

		// if ts is present, means we're implicitly syncing own cursor as a result of debounced rAF
		if (ts != null) {
			// this is not technically a "mousemove" event, since it's debounced, rename to setCursor?
			// since this is internal, we can tweak it later
			sync.pub(mousemove, self, mouseLeft1, mouseTop1, plotWidCss, plotHgtCss, idx);

			if (cursorFocus) {
				setSeries(closestDist <= focus.prox ? closestSeries : null, {focus: true}, syncOpts.setSeries);
			}
		}

		ready && fire("setCursor");
	}

	let rect = null;

	function syncRect() {
		rect = over.getBoundingClientRect();
	}

	function mouseMove(e, src, _x, _y, _w, _h, _i) {
		if (cursor._lock)
			return;

		cacheMouse(e, src, _x, _y, _w, _h, _i, false, e != null);

		if (e != null) {
			if (cursorRaf == 0)
				cursorRaf = rAF(updateCursor);
		}
		else
			updateCursor(null, src);
	}

	function cacheMouse(e, src, _x, _y, _w, _h, _i, initial, snap) {
		if (e != null) {
			_x = e.clientX - rect.left;
			_y = e.clientY - rect.top;
		}
		else {
			if (_x < 0 || _y < 0) {
				mouseLeft1 = -10;
				mouseTop1 = -10;
				return;
			}

			let [xKey, yKey] = syncOpts.scales;

			if (xKey != null)
				_x = getXPos(src.posToVal(_x, xKey), scales[xKey], plotWidCss, 0);
			else
				_x = plotWidCss * (_x/_w);

			if (yKey != null)
				_y = getYPos(src.posToVal(_y, yKey), scales[yKey], plotHgtCss, 0);
			else
				_y = plotHgtCss * (_y/_h);
		}

		if (snap) {
			if (_x <= 1 || _x >= plotWidCss - 1)
				_x = incrRound(_x, plotWidCss);

			if (_y <= 1 || _y >= plotHgtCss - 1)
				_y = incrRound(_y, plotHgtCss);
		}

		if (initial) {
			rawMouseLeft0 = _x;
			rawMouseTop0 = _y;

			[mouseLeft0, mouseTop0] = cursor.move(self, _x, _y);
		}
		else {
			mouseLeft1 = _x;
			mouseTop1 = _y;
		}
	}

	function hideSelect() {
		setSelect({
			width: 0,
			height: 0,
		}, false);
	}

	function mouseDown(e, src, _x, _y, _w, _h, _i) {
		dragging = true;
		dragX = dragY = drag._x = drag._y = false;

		cacheMouse(e, src, _x, _y, _w, _h, _i, true, false);

		if (e != null) {
			onMouse(mouseup, doc, mouseUp);
			sync.pub(mousedown, self, mouseLeft0, mouseTop0, plotWidCss, plotHgtCss, null);
		}
	}

	function mouseUp(e, src, _x, _y, _w, _h, _i) {
		dragging = drag._x = drag._y = false;

		cacheMouse(e, src, _x, _y, _w, _h, _i, false, true);

		let hasSelect = select[WIDTH] > 0 || select[HEIGHT] > 0;

		hasSelect && setSelect(select);

		if (drag.setScale && hasSelect) {
		//	if (syncKey != null) {
		//		dragX = drag.x;
		//		dragY = drag.y;
		//	}

			if (dragX) {
				_setScale(xScaleKey,
					scaleValueAtPos(select[LEFT], xScaleKey),
					scaleValueAtPos(select[LEFT] + select[WIDTH], xScaleKey)
				);
			}

			if (dragY) {
				for (let k in scales) {
					let sc = scales[k];

					if (k != xScaleKey && sc.from == null && sc.min != inf) {
						_setScale(k,
							scaleValueAtPos(select[TOP] + select[HEIGHT], k),
							scaleValueAtPos(select[TOP], k)
						);
					}
				}
			}

			hideSelect();
		}
		else if (cursor.lock) {
			cursor._lock = !cursor._lock;

			if (!cursor._lock)
				updateCursor();
		}

		if (e != null) {
			offMouse(mouseup, doc);
			sync.pub(mouseup, self, mouseLeft1, mouseTop1, plotWidCss, plotHgtCss, null);
		}
	}

	function mouseLeave(e, src, _x, _y, _w, _h, _i) {
		if (!cursor._lock) {
			let _dragging = dragging;

			if (dragging) {
				// handle case when mousemove aren't fired all the way to edges by browser
				let snapX = true;
				let snapY = true;
				let snapProx = 10;

				if (dragX && dragY) {
					// maybe omni corner snap
					snapX = mouseLeft1 <= snapProx || mouseLeft1 >= plotWidCss - snapProx;
					snapY = mouseTop1  <= snapProx || mouseTop1  >= plotHgtCss - snapProx;
				}

				if (dragX && snapX) {
					let dLft = mouseLeft1;
					let dRgt = plotWidCss - mouseLeft1;

					let xMin = min(dLft, dRgt);

					if (xMin == dLft)
						mouseLeft1 = 0;
					if (xMin == dRgt)
						mouseLeft1 = plotWidCss;
				}

				if (dragY && snapY) {
					let dTop = mouseTop1;
					let dBtm = plotHgtCss - mouseTop1;

					let yMin = min(dTop, dBtm);

					if (yMin == dTop)
						mouseTop1 = 0;
					if (yMin == dBtm)
						mouseTop1 = plotHgtCss;
				}

				updateCursor(1);

				dragging = false;
			}

			mouseLeft1 = -10;
			mouseTop1 = -10;

			// passing a non-null timestamp to force sync/mousemove event
			updateCursor(1);

			if (_dragging)
				dragging = _dragging;
		}
	}

	function dblClick(e, src, _x, _y, _w, _h, _i) {
		autoScaleX();

		hideSelect();

		if (e != null)
			sync.pub(dblclick, self, mouseLeft1, mouseTop1, plotWidCss, plotHgtCss, null);
	}

	// internal pub/sub
	const events = {};

	events[mousedown] = mouseDown;
	events[mousemove] = mouseMove;
	events[mouseup] = mouseUp;
	events[dblclick] = dblClick;
	events["setSeries"] = (e, src, idx, opts) => {
		setSeries(idx, opts);
	};

	let deb;

	if ( cursor.show) {
		onMouse(mousedown, over, mouseDown);
		onMouse(mousemove, over, mouseMove);
		onMouse(mouseenter, over, syncRect);
		// this has to be rAF'd so it always fires after the last queued/rAF'd updateCursor
		onMouse(mouseleave, over, e => { rAF(mouseLeave); });

		onMouse(dblclick, over, dblClick);

		deb = debounce(syncRect, 100);

		on(resize, win, deb);
		on(scroll, win, deb);

		self.syncRect = syncRect;
	}

	// external on/off
	const hooks = self.hooks = opts.hooks || {};

	function fire(evName, a1, a2) {
		if (evName in hooks) {
			hooks[evName].forEach(fn => {
				fn.call(null, self, a1, a2);
			});
		}
	}

	(opts.plugins || []).forEach(p => {
		for (let evName in p.hooks)
			hooks[evName] = (hooks[evName] || []).concat(p.hooks[evName]);
	});

	const syncOpts =  assign({
		key: null,
		setSeries: false,
		scales: [xScaleKey, null]
	}, cursor.sync);

	const syncKey =  syncOpts.key;

	const sync =  (syncKey != null ? (syncs[syncKey] = syncs[syncKey] || _sync()) : _sync());

	 sync.sub(self);

	function pub(type, src, x, y, w, h, i) {
		events[type](null, src, x, y, w, h, i);
	}

	 (self.pub = pub);

	function destroy() {
		 sync.unsub(self);
		 off(resize, win, deb);
		 off(scroll, win, deb);
		root.remove();
		fire("destroy");
	}

	self.destroy = destroy;

	function _init() {
		fire("init", opts, data);

		setData(data || opts.data, false);

		if (pendScales[xScaleKey])
			setScale(xScaleKey, pendScales[xScaleKey]);
		else
			autoScaleX();

		_setSize(opts[WIDTH], opts[HEIGHT]);

		setSelect(select, false);
	}

	if (then) {
		if (then instanceof HTMLElement) {
			then.appendChild(root);
			_init();
		}
		else
			then(self, _init);
	}
	else
		_init();

	return self;
}

uPlot.assign = assign;
uPlot.fmtNum = fmtNum;
uPlot.rangeNum = rangeNum;
uPlot.rangeLog = rangeLog;

{
	uPlot.fmtDate = fmtDate;
	uPlot.tzDate  = tzDate;
}

export default uPlot;
