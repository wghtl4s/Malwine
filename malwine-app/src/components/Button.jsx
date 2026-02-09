import React from 'react';
import classNames from 'classnames';
import '../scss/button.scss';
const Button = ({ onClick, className, children }) => {
  return (
    <button onClick={onClick} className={classNames('button', className)}>
      {children}
    </button>
  );
};

export default Button;
